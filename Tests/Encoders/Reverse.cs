using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Urchin.Interfaces;
using Urchin;


namespace Tests.Encoders
{
    [TestClass, TestCategory("Encoders.Reverse")]
    public class Reverse : Generic<Urchin.Encoders.Reverse>
    {
        [TestMethod]
        public override void Property_GetSet_WordSize()
        {
            base.Property_GetSet_WordSize();
        }

        [TestMethod]
        public override void Property_Get_SeedSize()
        {
            base.Property_Get_SeedSize();
        }

        [TestMethod]
        public override void Property_GetSet_Seed()
        {
            base.Property_GetSet_Seed();
        }

        [TestMethod]
        public override void Property_GetSet_Entropy()
        {
            base.Property_GetSet_Entropy();
        }

        [TestMethod]
        public override void Decode()
        {
            base.Decode();
        }

        [TestMethod]
        public override void Encode()
        {
            base.Encode();
        }

        [TestMethod]
        public override void LastWordEncode()
        {
            base.LastWordEncode();
        }

        [TestMethod]
        public override void LastWordDecode()
        {
            base.LastWordDecode();
        }

    }
}
