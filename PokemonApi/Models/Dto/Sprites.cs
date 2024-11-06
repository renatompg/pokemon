using Newtonsoft.Json;
namespace PokemonApi.Models.Dto
{
    public class Sprites
    {
        public string BackDefault64 { get; set; }
        public string FrontDefault64 { get; set; }

        [JsonProperty("back_default")]
        public string BackDefault { get; set; }
        [JsonProperty("back_female")]
        public string BackFemale { get; set; }

        [JsonProperty("back_shiny")]
        public string BackShiny { get; set; }

        [JsonProperty("back_shiny_female")]
        public string BackShinyFemale { get; set; }

        [JsonProperty("front_default")]
        public string FrontDefault { get; set; }

        [JsonProperty("front_female")]
        public string FrontFemale { get; set; }

        [JsonProperty("front_shiny")]
        public string FrontShiny { get; set; }

        [JsonProperty("front_shiny_female")]
        public string FrontShinyFemale { get; set; }
        public OtherSprites Other { get; set; }
        public Versions Versions { get; set; }
    }
    public class OtherSprites
    {
        public DreamWorld DreamWorld { get; set; }
        public Home Home { get; set; }
        public OfficialArtwork OfficialArtwork { get; set; }
        public Showdown Showdown { get; set; }
    }
    public class DreamWorld
    {
        [JsonProperty("front_default")]
        public string FrontDefault { get; set; }

        [JsonProperty("front_female")]
        public string FrontFemale { get; set; }
    }

    public class Home
    {
        [JsonProperty("front_default")]
        public string FrontDefault { get; set; }

        [JsonProperty("front_female")]
        public string FrontFemale { get; set; }

        [JsonProperty("front_shiny")]
        public string FrontShiny { get; set; }

        [JsonProperty("front_shiny_female")]
        public string FrontShinyFemale { get; set; }
    }

    public class OfficialArtwork
    {
        [JsonProperty("front_default")]
        public string FrontDefault { get; set; }

        [JsonProperty("front_shiny")]
        public string FrontShiny { get; set; }
    }

    public class Showdown
    {
        [JsonProperty("back_default")]
        public string BackDefault { get; set; }

        [JsonProperty("back_female")]
        public string BackFemale { get; set; }

        [JsonProperty("back_shiny")]
        public string BackShiny { get; set; }

        [JsonProperty("back_shiny_female")]
        public string BackShinyFemale { get; set; }

        [JsonProperty("front_default")]
        public string FrontDefault { get; set; }

        [JsonProperty("front_female")]
        public string FrontFemale { get; set; }

        [JsonProperty("front_shiny")]
        public string FrontShiny { get; set; }

        [JsonProperty("front_shiny_female")]
        public string FrontShinyFemale { get; set; }
    }

    public class Versions
    {
        [JsonProperty("generation-i")]
        public GenerationI GenerationI { get; set; }

        [JsonProperty("generation-ii")]
        public GenerationII GenerationII { get; set; }

        [JsonProperty("generation-iii")]
        public GenerationIII GenerationIII { get; set; }

        [JsonProperty("generation-iv")]
        public GenerationIV GenerationIV { get; set; }

        [JsonProperty("generation-v")]
        public GenerationV GenerationV { get; set; }
    }

    public class GenerationI
    {
        [JsonProperty("red-blue")]
        public GenerationSprites RedBlue { get; set; }

        [JsonProperty("yellow")]
        public GenerationSprites Yellow { get; set; }
    }

    public class GenerationII
    {
        [JsonProperty("crystal")]
        public GenerationSprites Crystal { get; set; }

        [JsonProperty("gold")]
        public GenerationSprites Gold { get; set; }

        [JsonProperty("silver")]
        public GenerationSprites Silver { get; set; }
    }

    public class GenerationIII
    {
        [JsonProperty("emerald")]
        public GenerationSprites Emerald { get; set; }

        [JsonProperty("firered-leafgreen")]
        public GenerationSprites FireredLeafgreen { get; set; }

        [JsonProperty("ruby-sapphire")]
        public GenerationSprites RubySapphire { get; set; }
    }

    public class GenerationIV
    {
        [JsonProperty("diamond-pearl")]
        public GenerationSprites DiamondPearl { get; set; }

        [JsonProperty("heartgold-soulsilver")]
        public GenerationSprites HeartgoldSoulsilver { get; set; }

        [JsonProperty("platinum")]
        public GenerationSprites Platinum { get; set; }
    }

    public class GenerationV
    {
        [JsonProperty("black-white")]
        public BlackWhite BlackWhite { get; set; }
    }

    public class GenerationSprites
    {
        [JsonProperty("back_default")]
        public string BackDefault { get; set; }

        [JsonProperty("back_gray")]
        public string BackGray { get; set; }

        [JsonProperty("back_transparent")]
        public string BackTransparent { get; set; }

        [JsonProperty("front_default")]
        public string FrontDefault { get; set; }

        [JsonProperty("front_gray")]
        public string FrontGray { get; set; }

        [JsonProperty("front_transparent")]
        public string FrontTransparent { get; set; }
    }

    public class BlackWhite
    {
        [JsonProperty("animated")]
        public Animated Animated { get; set; }
    }

    public class Animated
    {
        [JsonProperty("back_default")]
        public string BackDefault { get; set; }

        [JsonProperty("back_female")]
        public string BackFemale { get; set; }

        [JsonProperty("back_shiny")]
        public string BackShiny { get; set; }

        [JsonProperty("back_shiny_female")]
        public string BackShinyFemale { get; set; }

        [JsonProperty("front_default")]
        public string FrontDefault { get; set; }

        [JsonProperty("front_female")]
        public string FrontFemale { get; set; }

        [JsonProperty("front_shiny")]
        public string FrontShiny { get; set; }

        [JsonProperty("front_shiny_female")]
        public string FrontShinyFemale { get; set; }
    }
    public class Other
    {
        [JsonProperty("dream_world")]
        public DreamWorld DreamWorld { get; set; }

        [JsonProperty("home")]
        public Home Home { get; set; }

        [JsonProperty("official-artwork")]
        public OfficialArtwork OfficialArtwork { get; set; }

        [JsonProperty("showdown")]
        public Showdown Showdown { get; set; }
    }
}