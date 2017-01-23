using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace IsolatedIslandGame.Library.UnitTest
{
    [TestClass]
    public class BlueprintTest
    {
        [TestMethod]
        public void IsSufficientRequirements_MethodTest1()
        {
            Blueprint blueprint = new Blueprint(1, false, false, new Blueprint.ElementInfo[]
            {
                new Blueprint.ElementInfo { itemID = 1, itemCount = 5, positionIndex = 1 }
            }, new Blueprint.ElementInfo[]
            {
                new Blueprint.ElementInfo { itemID = 2, itemCount = 1, positionIndex = 1 }
            });

            Assert.IsTrue(blueprint.IsSufficientRequirements(new List<Blueprint.ElementInfo> { new Blueprint.ElementInfo { itemID = 1, itemCount = 5, positionIndex = 1 } }));
        }
        [TestMethod]
        public void IsSufficientRequirements_MethodTest2()
        {
            Blueprint blueprint = new Blueprint(1, false, false, new Blueprint.ElementInfo[]
            {
                new Blueprint.ElementInfo { itemID = 1, itemCount = 5, positionIndex = 1 }
            }, new Blueprint.ElementInfo[]
            {
                new Blueprint.ElementInfo { itemID = 2, itemCount = 1, positionIndex = 1 }
            });

            Assert.IsFalse(blueprint.IsSufficientRequirements(new List<Blueprint.ElementInfo> { new Blueprint.ElementInfo { itemID = 1, itemCount = 5, positionIndex = 2 } }));
        }
        [TestMethod]
        public void IsSufficientRequirements_MethodTest3()
        {
            Blueprint blueprint = new Blueprint(1, false, false, new Blueprint.ElementInfo[]
            {
                new Blueprint.ElementInfo { itemID = 1, itemCount = 2, positionIndex = 1 },
                new Blueprint.ElementInfo { itemID = 1, itemCount = 2, positionIndex = 2 }
            }, new Blueprint.ElementInfo[]
            {
                new Blueprint.ElementInfo { itemID = 2, itemCount = 2, positionIndex = 1 }
            });

            Assert.IsFalse(blueprint.IsSufficientRequirements(new List<Blueprint.ElementInfo> { new Blueprint.ElementInfo { itemID = 1, itemCount = 2, positionIndex = 1 } }));
            Assert.IsFalse(blueprint.IsSufficientRequirements(new List<Blueprint.ElementInfo> { new Blueprint.ElementInfo { itemID = 1, itemCount = 2, positionIndex = 2 } }));
            Assert.IsTrue(blueprint.IsSufficientRequirements(new List<Blueprint.ElementInfo>
            {
                new Blueprint.ElementInfo { itemID = 1, itemCount = 2, positionIndex = 1 },
                new Blueprint.ElementInfo { itemID = 1, itemCount = 2, positionIndex = 2 }
            }));
            Assert.IsTrue(blueprint.IsSufficientRequirements(new List<Blueprint.ElementInfo>
            {
                new Blueprint.ElementInfo { itemID = 1, itemCount = 2, positionIndex = 2 },
                new Blueprint.ElementInfo { itemID = 1, itemCount = 2, positionIndex = 1 }
            }));
        }
        [TestMethod]
        public void IsSufficientRequirements_MethodTest4()
        {
            Blueprint blueprint = new Blueprint(1, false, false, new Blueprint.ElementInfo[]
            {
                new Blueprint.ElementInfo { itemID = 1, itemCount = 2, positionIndex = 1 },
                new Blueprint.ElementInfo { itemID = 2, itemCount = 4, positionIndex = 2 }
            }, new Blueprint.ElementInfo[]
            {
                new Blueprint.ElementInfo { itemID = 2, itemCount = 2, positionIndex = 1 }
            });

            Assert.IsFalse(blueprint.IsSufficientRequirements(new List<Blueprint.ElementInfo>
            {
                new Blueprint.ElementInfo { itemID = 1, itemCount = 2, positionIndex = 2 },
                new Blueprint.ElementInfo { itemID = 2, itemCount = 4, positionIndex = 1 }
            }));
            Assert.IsTrue(blueprint.IsSufficientRequirements(new List<Blueprint.ElementInfo>
            {
                new Blueprint.ElementInfo { itemID = 1, itemCount = 2, positionIndex = 1 },
                new Blueprint.ElementInfo { itemID = 2, itemCount = 4, positionIndex = 2 }
            }));
        }
    }
}
