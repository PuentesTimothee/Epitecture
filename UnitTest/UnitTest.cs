
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using epitecture;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestAPI_Search()
        {
            ImgurAPI api = new ImgurAPI("3f640d9fa88b1d2", "a36a20ec9ac4cbe0f4e164c51ad2e81bc05570db");
            api.SearchGalleryAsync("Test").Wait();
        }

        [TestMethod]
        public void TestAPI_Random()
        {
            ImgurAPI api = new ImgurAPI("3f640d9fa88b1d2", "a36a20ec9ac4cbe0f4e164c51ad2e81bc05570db");
            api.GetRandomGalleryAsync().Wait();
        }


    }
}
