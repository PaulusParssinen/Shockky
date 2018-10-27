using System;

using Shockky.Shockwave.Chunks;

namespace Shockky.Shockwave.Lingo
{
    public abstract class LingoItem : ShockwaveItem
    {
        public ScriptChunk Script { get; }

        protected LingoItem(ScriptChunk script)
        {
            Script = script;
        }
        
        public virtual string ToLingo()
        {
            throw new NotSupportedException();
        }
    }
}