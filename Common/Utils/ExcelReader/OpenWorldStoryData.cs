using Newtonsoft.Json;

namespace Common.Utils.ExcelReader
{
    public class OpenWorldStoryData : BaseExcelReader<OpenWorldStoryData, OpenWorldStoryDataExcel>
    {
        public override string FileName { get { return "OpenWorldStoryData.json"; } }

        public OpenWorldStoryDataExcel? FromId(int id)
        {
            return All.Where(story => story.StoryId == id).FirstOrDefault();
        }
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public partial class OpenWorldStoryDataExcel
    {
        [JsonProperty("StorySeriesID")]
        public int StorySeriesId { get; set; }

        [JsonProperty("StorySeriesStep")]
        public int StorySeriesStep { get; set; }

        [JsonProperty("StorySeriesTitle")]
        public HashName StorySeriesTitle { get; set; }

        [JsonProperty("PreStory")]
        public int[] PreStory { get; set; }

        [JsonProperty("StoryType")]
        public int StoryType { get; set; }

        [JsonProperty("GroupType")]
        public int GroupType { get; set; }

        [JsonProperty("Cycle")]
        public int Cycle { get; set; }

        [JsonProperty("StoryStartDate")]
        public string StoryStartDate { get; set; }

        [JsonProperty("UnlockMapLv")]
        public int UnlockMapLv { get; set; }

        [JsonProperty("UnlockQuestLv")]
        public int UnlockQuestLv { get; set; }

        [JsonProperty("ShowConditionList")]
        public string ShowConditionList { get; set; }

        [JsonProperty("isUseNewCondition")]
        public bool IsUseNewCondition { get; set; }

        [JsonProperty("UnlockConditionList")]
        public string UnlockConditionList { get; set; }

        [JsonProperty("PreviewRelateSeries")]
        public int[] PreviewRelateSeries { get; set; }

        [JsonProperty("UnlockConditionTips")]
        public HashName UnlockConditionTips { get; set; }

        [JsonProperty("StoryMap")]
        public int StoryMap { get; set; }

        [JsonProperty("StoryArea")]
        public int StoryArea { get; set; }

        [JsonProperty("Name")]
        public HashName Name { get; set; }

        [JsonProperty("HuntRewardItem")]
        public int HuntRewardItem { get; set; }

        [JsonProperty("HuntRewardItemDisplay")]
        public int HuntRewardItemDisplay { get; set; }

        [JsonProperty("Description")]
        public HashName Description { get; set; }

        [JsonProperty("Target")]
        public HashName Target { get; set; }

        [JsonProperty("MaxCount")]
        public int MaxCount { get; set; }

        [JsonProperty("LocationID")]
        public int LocationId { get; set; }

        [JsonProperty("DLCChallengeMode")]
        public bool DlcChallengeMode { get; set; }

        [JsonProperty("IsTaskAnimation")]
        public int IsTaskAnimation { get; set; }

        [JsonProperty("IsHideUI")]
        public bool IsHideUi { get; set; }

        [JsonProperty("IsTutorial")]
        public bool IsTutorial { get; set; }

        [JsonProperty("PreStage")]
        public object[] PreStage { get; set; }

        [JsonProperty("DataImpl")]
        public object DataImpl { get; set; }

        [JsonProperty("StoryID")]
        public int StoryId { get; set; }
    }
}
