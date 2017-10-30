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

        public short DifficultyValue
        {
            get { return (short)Difficulty; }
        }

        public Func<Mission> OnGetMission;
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
            Solution.OnGetMissionStage += new Func<MissionStage>(delegate { return this; });
        }

        //public string SolutionDescription { get { return Solution.DescriptionWithMass(); } }
        public string SolutionDescription { get { return Solution.Description; } }

        public bool HasBeenSolved()
        {
            return Solution.IsSufficient();
        }

        public Color Colour { get { return HasBeenSolved() ? Color.LightGreen : Color.PaleVioletRed; } }
    }
}
