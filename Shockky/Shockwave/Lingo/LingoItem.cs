using System;
using Shockky.Shockwave.Chunks;

namespace Shockky.Shockwave.Lingo
{
    public abstract class LingoItem : ShockwaveItem
    {
        protected ScriptChunk Script { get; }

        protected LingoItem(ScriptChunk script)
        {
            Script = script;
        }

        public ScriptChunk GetScript() => Script;

        public virtual string ToLingo()
        {
            throw new NotSupportedException();
        }
    }
}
