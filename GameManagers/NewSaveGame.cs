using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
    public enum Characters
    {
        Default,
        BlueBooky,
        GreenBooky,
        PinkBooky,
        PurpleBooky,
        YellowBooky,
        BlackOutBooky,
        ChromeBooky,
        EndBooky,
        GoldBooky,
        ZombieBooky,
        WightBooky,
        NeonBooky,
        RainbowBooky,
        GhostBooky,
        DiamondBooky,
        CandyBooky,
        AmberBooky,
        AmethystBooky,
        JadeBooky,
        RubyBooky,
        TopazBooky
    }

    public enum Hats
    {
        None,
        BlackHat,
        BlueHat,
        GreenHat,
        RedHat
    }

    public class NewSaveGame 
	{
        static protected NewSaveGame instance;
        public static NewSaveGame Instance
        {
            get
            {
                if (instance == null)
                {
                    Create();
                }
                return instance;
            }
        }

        protected string saveFile = "";

        public int pages;

        public List<int> ownedCharacters = new List<int>();
        public List<int> ownedHats = new List<int>();
        public string[] prefabs = new string[3];
        public int usedPrefab;

        public float masterVolume, musicVolume, masterSFXVolume, runVolume, glideVolume;

        public int[,] achievementSave = new int[System.Enum.GetNames(typeof(Achievements)).Length, 2];

        public int timeSecondsOverflow;

        /// <summary>
        /// 0 = Do tutorial. 1 = Don't do tutorial.
        /// </summary>
        public int tutorial;

        /// <summary>
        /// 1 = Low. 0 = High.
        /// </summary>
        public int resolution;

        //1.1
        /// <summary>
        /// 0 = Locked. 1 = Unlocked
        /// </summary>
        public int fireImmuneUnlocked;

        /// <summary>
        /// 0 = NotActive. 1 = Active
        /// </summary>
        public int fireImmuneActive;

        /// <summary>
        /// 0 = Locked. 1 = Unlocked
        /// </summary>
        public int halfGlideGravityUnlocked;

        /// <summary>
        /// 0 = NotActive. 1 = Active
        /// </summary>
        public int halfGlideGravityActive;


        //1.1 Changed version to 2
        static int s_Version = 2;


        public void AddCharacter(int character)
        {
            ownedCharacters.Add(character);

            Instance.achievementSave[(int)Achievements.UnlockedSkins, 0] = Instance.ownedCharacters.Count;

            Save();
        }

        public void SetActivePrefab(int prefab)
        {
            if (prefab >= 0 && prefab < prefabs.Length)
            {
                usedPrefab = prefab;
                Save();
            }
        }

        public void NewHighScore(int score)
        {
            if (score > achievementSave[(int)Achievements.HighScore, 0])
            {
                achievementSave[(int)Achievements.HighScore, 0] = score;
            }

            achievementSave[(int)Achievements.DistanceTotal, 0] += score;

            Save();
        }

        public void HandlePagesCollectedAchievement(int pages)
        {
            if (pages > achievementSave[(int)Achievements.PagesCollected1Life, 0])
            {
                achievementSave[(int)Achievements.PagesCollected1Life, 0] = pages;
            }
            achievementSave[(int)Achievements.PagesCollectedTotal, 0] += pages;
        }

        public void AddJumps(int jumps)
        {
            if (jumps > achievementSave[(int)Achievements.JumpsIn1Life, 0])
            {
                achievementSave[(int)Achievements.JumpsIn1Life, 0] = jumps;
            }
            achievementSave[(int)Achievements.JumpsTotal, 0] += jumps;
        }

        public void AddGlides(int glides)
        {
            if (glides > achievementSave[(int)Achievements.GlidesIn1Life, 0])
            {
                achievementSave[(int)Achievements.GlidesIn1Life, 0] = glides;
            }
            achievementSave[(int)Achievements.GlidesTotal, 0] += glides;
        }

        public void AddTimePlayed(int time)
        {
            int timeToAdd = 0;
            timeSecondsOverflow += time % 60;
            if (timeSecondsOverflow >= 60)
            {
                timeToAdd++;
                timeSecondsOverflow -= 60;
            }

            timeToAdd += time / 60;

            achievementSave[(int)Achievements.MinutesPlayedTotal, 0] += timeToAdd;
        }

        public void AddSuperJumpWhileGliding(int jumps)
        {
            achievementSave[(int)Achievements.SuperJumpWhileGliding, 0] += jumps;
        }

        public void TutorialFinished()
        {
            tutorial = 1;
            Save();
        }

        static public void Create()
        {
            if (instance == null)
            {
                instance = new NewSaveGame();
            }

            Instance.saveFile = Application.persistentDataPath + "/BookItRunnerSave.bin";

            if (File.Exists(Instance.saveFile))
            {
                // If we have a save, we read it.
                Instance.Read();
            }
            else
            {
                // If not we create one with default data.
                NewSave();
            }
        }

        static public void NewSave()
        {
            Instance.ownedCharacters.Clear();

            Instance.pages = 0;

            Instance.ownedCharacters.Add(0);

            Instance.ownedHats.Add(0);

            for (int i = 0; i < Instance.prefabs.Length; i++)
            {
                Instance.prefabs[i] = "0,0";
            }

            Instance.usedPrefab = 0;

            Instance.masterVolume = -80f;
            Instance.musicVolume = -80f;
            Instance.masterSFXVolume = -80f;
            Instance.runVolume = -80f;
            Instance.glideVolume = -80f;

            for (int x = 0; x < Instance.achievementSave.GetLength(0); x++)
            {
                Instance.achievementSave[x, 0] = 0;
                Instance.achievementSave[x, 1] = 0;
            }

            Instance.achievementSave[(int)Achievements.UnlockedSkins, 0] = Instance.ownedCharacters.Count;

            Instance.timeSecondsOverflow = 0;

            Instance.tutorial = 0;

            Instance.resolution = 0;

            Instance.fireImmuneUnlocked = 0;
            Instance.fireImmuneActive = 0;

            Instance.halfGlideGravityUnlocked = 0;
            Instance.halfGlideGravityActive = 0;

            Instance.Save();
        }

        public void Save()
        {
            BinaryWriter w = new BinaryWriter(new FileStream(saveFile, FileMode.OpenOrCreate));

            w.Write(s_Version); //int
            w.Write(pages); //int

            // Write characters.
            w.Write(ownedCharacters.Count); //int
            foreach (int c in ownedCharacters)
            {
                w.Write(c); //int
            }

            // Write Hats
            w.Write(ownedHats.Count); //int
            foreach (int h in ownedHats)
            {
                w.Write(h); //int
            }

            // Write prefabs
            for (int i = 0; i < prefabs.Length; i++)
            {
                w.Write(prefabs[i]); //string
            }

            w.Write(usedPrefab); //int

            w.Write(masterVolume);
            w.Write(musicVolume);
            w.Write(masterSFXVolume);
            w.Write(runVolume);
            w.Write(glideVolume);

            w.Write(achievementSave.GetLength(0));
            for (int i = 0; i < achievementSave.GetLength(0); i++)
            {
                w.Write(achievementSave[i, 0]);
                w.Write(achievementSave[i, 1]);
            }

            w.Write(timeSecondsOverflow);

            w.Write(tutorial);

            w.Write(resolution);

            w.Write(fireImmuneUnlocked);
            w.Write(fireImmuneActive);

            w.Write(halfGlideGravityUnlocked);
            w.Write(halfGlideGravityActive);

            w.Close();
        }

        public void Read()
        {
            BinaryReader r = new BinaryReader(new FileStream(saveFile, FileMode.Open));

            int ver = r.ReadInt32();

            pages = r.ReadInt32();

            // Read character.
            ownedCharacters.Clear();
            int charCount = r.ReadInt32();
            for (int i = 0; i < charCount; ++i)
            {
                ownedCharacters.Add(r.ReadInt32());
            }

            // Read hats
            ownedHats.Clear();
            int hatCount = r.ReadInt32();
            for (int i = 0; i < hatCount; i++)
            {
                ownedHats.Add(r.ReadInt32());
            }

            for (int i = 0; i < prefabs.Length; i++)
            {
                prefabs[i] = r.ReadString();
            }

            usedPrefab = r.ReadInt32();

            masterVolume = r.ReadSingle();
            musicVolume = r.ReadSingle();
            masterSFXVolume = r.ReadSingle();
            runVolume = r.ReadSingle();
            glideVolume = r.ReadSingle();

            int achievementCount = r.ReadInt32();
            for (int i = 0; i < achievementCount; i++)
            {
                achievementSave[i, 0] = r.ReadInt32();
                achievementSave[i, 1] = r.ReadInt32();
            }

            timeSecondsOverflow = r.ReadInt32();

            tutorial = r.ReadInt32();

            resolution = r.ReadInt32();

            if (ver >= 2)
            {
                fireImmuneUnlocked = r.ReadInt32();
                fireImmuneActive = r.ReadInt32();

                halfGlideGravityUnlocked = r.ReadInt32();
                halfGlideGravityActive = r.ReadInt32();
            }
   
            r.Close();
        }
    }
}

