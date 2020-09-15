// #define LFE_DEBUG
// https://github.com/CraftyMoment/mmd_vam_import/blob/master/vmd.py

using System;
using System.Linq;
using MVR.FileManagementSecure;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using LFE.MMD;

namespace LFE
{
    public class FreeMMD : MVRScript
    {
        public override void Init()
        {
            CreateUI();
        }

        public void OnEnable()
        {
            StopAndStartOver();
        }

        public void OnDisable()
        {
            StopAndStartOver();
        }

        private bool isPlaying = false;
        private float currentTime = 0f;

        public void FixedUpdate()
        {
            if (!isPlaying)
            {
                return;
            }

            if (SuperController.singleton.freezeAnimation)
            {
                return;
            }

            if (!SuperController.singleton.isActiveAndEnabled)
            {
                return;
            }


            var maxTime = MotionTrack.MaxFrame / VmdFile.Fps;
            var headAudioSource = containingAtom.GetStorableByID("HeadAudioSource") as AudioSourceControl;
            if (AudioClip != null && headAudioSource != null && headAudioSource.audioSource != null)
            {
                currentTime += Time.fixedDeltaTime;
            }
            else
            {
                currentTime += Time.fixedDeltaTime;
            }

            // TOOD: allow looping later -- this assume we end
            if (currentTime > maxTime)
            {
                StopAndStartOver();
            }

            try
            {
                // SuperController.singleton.ClearMessages();

                var frames = MotionTrack.VamPoseAtTime(currentTime);
                foreach (var frame in frames)
                {
                    if (MotionTrack.ControllersByName.ContainsKey(frame.ControllerName))
                    {
                        var controller = MotionTrack.ControllersByName[frame.ControllerName];

                        var doPosition = new string[] { "hipControl", "lFootControl", "rFootControl" }.Contains(frame.ControllerName);
                        if (doPosition)
                        {
                            controller.transform.localPosition = frame.Position;
                        }
                        if (frame.Frame == 0 && frame.Rotation.x == 0 && frame.Rotation.y == 0 && frame.Rotation.z == 0)
                        {
                            // skip rotation if first frame and all 0s
                        }
                        else
                        {
                            controller.transform.localRotation = frame.Rotation;
                        }
                    }
                    else if (MotionTrack.BonesByName.ContainsKey(frame.ControllerName))
                    {
                        var bone = MotionTrack.BonesByName[frame.ControllerName];
                        bone.transform.localRotation = frame.Rotation;
                    }
                }

                // TODO: map to morphs
                // var faces = MotionTrack.VamFacesAtTime(currentTime);
                // foreach(var item in faces) {
                //     var faceName = item.Key.ToEnglishName();
                //     var data = item.Value;
                //     SuperController.LogMessage($"{faceName}: v={data.Rate}");
                // }
            }
            catch (Exception e)
            {
                SuperController.LogError(e.ToString());
                isPlaying = false;
            }

        }


        // ----------------------------------------

        public void ResetPose()
        {

            AtomUI componentInChildren = containingAtom.GetComponentInChildren<AtomUI>();
            if (componentInChildren != null)
            {
                // SuperController.LogMessage("resetting pose");
                componentInChildren?.resetButton?.onClick?.Invoke();
            }

        }

        public bool TogglePlaying()
        {
            if (isPlaying)
            {
                StopPlaying();
            }
            else
            {
                StartPlaying();
            }

            return isPlaying;
        }

        public void StopPlaying()
        {
            isPlaying = false;

            var headAudioSource = containingAtom.GetStorableByID("HeadAudioSource") as AudioSourceControl;
            if (headAudioSource != null)
            {
                headAudioSource.Stop();
            }

            if (UIPlayButton != null)
            {
                UIPlayButton.buttonText.text = "Play";
            }
        }

        public void StopAndStartOver()
        {
            StopPlaying();
            StartOver();
        }

        public void StartPlaying()
        {
            isPlaying = true;
            var headAudioSource = containingAtom.GetStorableByID("HeadAudioSource") as AudioSourceControl;
            if (headAudioSource != null && AudioClip != null)
            {
                headAudioSource.PlayNowClearQueue(AudioClip);
                headAudioSource.audioSource.time = currentTime;
            }
            if (UIPlayButton != null)
            {
                UIPlayButton.buttonText.text = "Pause";
            }
        }

        public void StartOver()
        {
            SetTime(0);
        }

        public void SetTime(float time)
        {
            if (time < 0) { time = 0; }
            var headAudioSource = containingAtom.GetStorableByID("HeadAudioSource") as AudioSourceControl;
            if (headAudioSource != null && AudioClip != null)
            {
                headAudioSource.audioSource.time = time;
            }
            currentTime = time;
        }

        public MotionTrack MotionTrack;
        public NamedAudioClip AudioClip;
        public JSONStorableString StorableMotionFileName;
        public JSONStorableString StorableMusicFileName;
        public JSONStorableFloat StorableHipOffsetY;
        public JSONStorableFloat StorableHipOffsetZ;
        public UIDynamicButton UIMotionFileButton;
        public UIDynamicButton UIAudioFileButton;
        public UIDynamicButton UIExportButton;
        public UIDynamicButton UIPlayButton;
        public UIDynamicSlider UIHipOffsetY;
        public UIDynamicSlider UIHipOffsetZ;
        public void CreateUI()
        {
            // motion file
            StorableMotionFileName = new JSONStorableString("motionFileName", String.Empty);

            var fileText = CreateTextField(StorableMotionFileName, rightSide: false);
            fileText.height = 100;

            UIMotionFileButton = CreateButton("Motion File...", rightSide: true);
            UIMotionFileButton.height = 100;
            UIMotionFileButton.button.onClick.AddListener(() =>
            {
                try
                {
                    SuperController.singleton.GetMediaPathDialog(
                        (p) =>
                        {
                            try
                            {
                                StopAndStartOver();
                                ResetPose();
                                var vmdFile = VmdFile.Read(p);
                                MotionTrack = new MotionTrack(vmdFile, containingAtom, StorableHipOffsetY.val, StorableHipOffsetZ.val);
                                StorableMotionFileName.val = p;
                            }
                            catch (Exception e)
                            {
                                SuperController.LogError(e.ToString());
                            }
                        },
                        filter: "vmd",
                        showInstallFolderInDirectoryList: true
                    );
                }
                catch (Exception e)
                {
                    SuperController.LogError(e.ToString());
                }
            });

            // audio file
            StorableMusicFileName = new JSONStorableString("soundFileName", String.Empty);

            var audioText = CreateTextField(StorableMusicFileName, rightSide: false);
            audioText.height = 100;

            UIAudioFileButton = CreateButton("Music File...", rightSide: true);
            UIAudioFileButton.height = 100;
            UIAudioFileButton.button.onClick.AddListener(() =>
            {
                try
                {
                    SuperController.singleton.GetMediaPathDialog(
                        (p) =>
                        {
                            try
                            {
                                StopAndStartOver();
                                AudioClip = LoadAudio(p);
                                StorableMusicFileName.val = p;
                            }
                            catch (Exception e)
                            {
                                SuperController.LogError(e.ToString());
                            }
                        },
                        filter: "wav|mp3|ogg",
                        showInstallFolderInDirectoryList: true
                    );
                }
                catch (Exception e)
                {
                    SuperController.LogError(e.ToString());
                }
            });

            // play button
            UIPlayButton = CreateButton("Play", rightSide: true);
            UIPlayButton.height = 100;
            UIPlayButton.button.onClick.AddListener(() =>
            {
                TogglePlaying();
            });

            StorableHipOffsetY = new JSONStorableFloat("hipOffsetY", -0.05f, (float value) =>
            {
                if (MotionTrack != null)
                {
                    MotionTrack.HipOffsetY = value;
                }
            }, -1, 1);
            UIHipOffsetY = CreateSlider(StorableHipOffsetY);

            StorableHipOffsetZ = new JSONStorableFloat("hipOffsetZ", 0, (float value) =>
            {
                if (MotionTrack != null)
                {
                    MotionTrack.HipOffsetZ = value;
                }
            }, -1, 1);
            UIHipOffsetZ = CreateSlider(StorableHipOffsetZ);
        }

        public static NamedAudioClip LoadAudio(string path)
        {
            string localPath = SuperController.singleton.NormalizeLoadPath(path);
            NamedAudioClip existing = URLAudioClipManager.singleton.GetClip(localPath);
            if (existing != null)
            {
                return existing;
            }

            URLAudioClip clip = URLAudioClipManager.singleton.QueueClip(SuperController.singleton.NormalizeMediaPath(path));
            if (clip == null)
            {
                return null;
            }

            NamedAudioClip nac = URLAudioClipManager.singleton.GetClip(clip.uid);
            if (nac == null)
            {
                return null;
            }
            return nac;
        }
    }
}
