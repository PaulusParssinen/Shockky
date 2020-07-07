using Shockky.Chunks;

namespace Shockky.Lingo
{
    public abstract class LingoItem : ShockwaveItem
    {
        public LingoScriptChunk Script { get; }

        protected LingoItem(LingoScriptChunk script)
        {
            Script = script;
        }
    }
}