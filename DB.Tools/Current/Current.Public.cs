namespace DB.Tools
{
  

    public partial class Current
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Current()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Current(ref BS bss, ref Interface interfaces)
        {
            bs = bss;
            Interface = interfaces;
        }
    }
}