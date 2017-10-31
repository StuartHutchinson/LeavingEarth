using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LeavingEarth
{
    public enum DifficultyLevel { Unset, One, Two, Three, Four, Five, Six, Seven, Eight, Nine }
    
    public class MissionStage
    {
        public string Description { get; set; }
        public DifficultyLevel Difficulty { get; set; }
        public short Payload { get; set; }
        public MissionStageSolution Solution { get; set; }

        [JsonIgnore]
        public short DifficultyValue
        {
            get { return (short)Difficulty; }
        }

        [JsonIgnore]
        public Func<Mission> OnGetMission;

        [JsonIgnore]
        public string SolutionDescription { get { return Solution.Description; } }

        [JsonIgnore]
        public Color Colour { get { return HasBeenSolved() ? Color.LightGreen : Color.PaleVioletRed; } }

        public Mission GetMission()
        {
            if (OnGetMission == null)
                throw new Exception("OnGetMission handler is not assigned");

            return OnGetMission();
        }

        public MissionStage()
        {
            Solution = new MissionStageSolution();
            //Solution.OnGetMissionStage += new Func<MissionStage>(delegate { return this; });
            Solution.OnGetMissionStage += new Func<MissionStage>(GetMissionStage);
        }

        private MissionStage GetMissionStage()
        {
            return this;
        }

        public MissionStage(string desc) : this()
        {
            Description = desc;
            //Solution = new MissionStageSolution(this);
        }

        public MissionStage (MissionStage original)
        {
            Description = original.Description;
            Difficulty = original.Difficulty;
            Payload = original.Payload;
            //Solution = (original.Solution != null) ?
            //new MissionStageSolution(original.Solution) :
            //null;
            Solution = new MissionStageSolution(original.Solution);
            Solution.OnGetMissionStage += new Func<MissionStage>(GetMissionStage);
            OnGetMission = original.OnGetMission;
        }
        
        public bool HasBeenSolved()
        {
            return Solution.IsSufficient();
        }

        //unsubscribe the event so json doesn't attempt to serialize the function 
        public void PrepareForSave()
        {
            Solution.OnGetMissionStage -= GetMissionStage;
        }
    }
}
