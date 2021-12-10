using Server.Commands;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Server.Engines.UOStore
{
    public enum StoreCategory
    {
        None,
        Featured,
        Character,
        Equipment,
        Decorations,
        Mounts,
        Misc,
        Cart
    }

    public enum SortBy
    {
        Name,
        PriceLower,
        PriceHigher,
        Newest,
        Oldest
    }

    public static class UltimaStore
    {
        public static readonly string FilePath = Path.Combine("Saves", "Misc", "UltimaStore.bin");

        public static bool Enabled { get { return Configuration.Enabled; } set { Configuration.Enabled = value; } }

        public static List<StoreEntry> Entries { get; private set; }
        public static Dictionary<Mobile, List<Item>> PendingItems { get; private set; }

        private static UltimaStoreContainer _UltimaStoreContainer;

        public static UltimaStoreContainer UltimaStoreContainer
        {
            get
            {
                if (_UltimaStoreContainer != null && _UltimaStoreContainer.Deleted)
                {
                    _UltimaStoreContainer = null;
                }

                return _UltimaStoreContainer ?? (_UltimaStoreContainer = new UltimaStoreContainer());
            }
        }

        static UltimaStore()
        {
            Entries = new List<StoreEntry>();
            PendingItems = new Dictionary<Mobile, List<Item>>();
            PlayerProfiles = new Dictionary<Mobile, PlayerProfile>();
        }

        public static void Configure()
        {
            PacketHandlers.Register(0xFA, 1, true, UOStoreRequest);

            CommandSystem.Register("Store", AccessLevel.Player, e => OpenStore(e.Mobile as PlayerMobile));

            EventSink.WorldSave += OnSave;
            EventSink.WorldLoad += OnLoad;
        }

        public static void Initialize()
        {
			StoreCategory cat;

			// Featured
			cat = StoreCategory.Featured;


            // Character
            cat = StoreCategory.Character;


            // Equipment
            cat = StoreCategory.Equipment;
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1070994 }, 1156906, 0, 0x9CA8, 0, 400, cat, ConstructPigments); // Nox Green
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1079584 }, 1156906, 0, 0x9CAF, 0, 400, cat, ConstructPigments); // Midnight Coal
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1070995 }, 1156906, 0, 0x9CA5, 0, 400, cat, ConstructPigments); // Rum Red
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1079580 }, 1156906, 0, 0x9CA4, 0, 400, cat, ConstructPigments); // Coal
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1079582 }, 1156906, 0, 0x9CA3, 0, 400, cat, ConstructPigments); // Storm Bronze
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1079581 }, 1156906, 0, 0x9CA2, 0, 400, cat, ConstructPigments); // Faded Gold
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1070988 }, 1156906, 0, 0x9CA1, 0, 400, cat, ConstructPigments); // Violet Courage Purple
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1079585 }, 1156906, 0, 0x9CA2, 0, 400, cat, ConstructPigments); // Faded Bronze
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1070996 }, 1156906, 0, 0x9C9F, 0, 400, cat, ConstructPigments); // Fire Orange
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1079586 }, 1156906, 0, 0x9C9E, 0, 400, cat, ConstructPigments); // Faded Rose
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1079583 }, 1156906, 0, 0x9CA7, 0, 400, cat, ConstructPigments); // Rose
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1079587 }, 1156906, 0, 0x9CA9, 0, 400, cat, ConstructPigments); // Deep Rose
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1070990 }, 1156906, 0, 0x9CAA, 0, 400, cat, ConstructPigments); // Luna White

            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1070992 }, 1156906, 0, 0x9CAF, 0, 400, cat, ConstructPigments); // Shadow Dancer Black
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1070989 }, 1156906, 0, 0x9CAE, 0, 400, cat, ConstructPigments); // Invulnerability Blue
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1070991 }, 1156906, 0, 0x9CAD, 0, 400, cat, ConstructPigments); // Dryad Green
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1070993 }, 1156906, 0, 0x9CAC, 0, 400, cat, ConstructPigments); // Berserker Red
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1079579 }, 1156906, 0, 0x9CAB, 0, 400, cat, ConstructPigments); // Faded Coal
            Register<PigmentsOfTokuno>(new TextDefinition[] { 1070933, 1070987 }, 1156906, 0, 0x9C9D, 0, 400, cat, ConstructPigments); // Paragon Gold

            Register<ReptalonFormTalisman>(new TextDefinition[] { 1157010, 1075202 }, 1156967, 0x2F59, 0, 0, 100, cat);
            Register<QuiverOfInfinity>(1075201, 1156971, 0x2B02, 0, 0, 100, cat);
            Register<CuSidheFormTalisman>(new TextDefinition[] { 1157010, 1031670 }, 1156970, 0x2F59, 0, 0, 100, cat);
            Register<FerretFormTalisman>(new TextDefinition[] { 1157010, 1031672 }, 1156969, 0x2F59, 0, 0, 100, cat);
            Register<LeggingsOfEmbers>(1062911, 1156956, 0x1411, 0, 0x2C, 100, cat);
            Register<ShaminoCrossbow>(1062915, 1156957, 0x26C3, 0, 0x504, 100, cat);
            Register<SamuraiHelm>(1062923, 1156959, 0x236C, 0, 0, 100, cat);
            Register<HolySword>(1062921, 1156962, 0xF61, 0, 0x482, 100, cat);
            Register<DupresShield>(1075196, 1156963, 0x2B01, 0, 0, 100, cat);
            Register<OssianGrimoire>(1078148, 1156965, 0x2253, 0, 0, 100, cat);
            Register<SquirrelFormTalisman>(new TextDefinition[] { 1157010, 1031671 }, 1156966, 0x2F59, 0, 0, 100, cat);
            Register<EarringsOfProtection>(new TextDefinition[] { 1156821, 1156822 }, 1156659, 0, 0x9C66, 0, 200, cat, ConstructEarrings); // Physcial
            Register<EarringsOfProtection>(1071092, 1156659, 0, 0x9C66, 0, 200, cat, ConstructEarrings); // Fire
            Register<EarringsOfProtection>(1071093, 1156659, 0, 0x9C66, 0, 200, cat, ConstructEarrings); // Cold
            Register<EarringsOfProtection>(1071094, 1156659, 0, 0x9C66, 0, 200, cat, ConstructEarrings); // Poison
            Register<EarringsOfProtection>(1071095, 1156659, 0, 0x9C66, 0, 200, cat, ConstructEarrings); // Energy
            Register<HoodedShroudOfShadows>(1079727, 1156643, 0x2684, 0, 0x455, 1000, cat);


            // decorations
            cat = StoreCategory.Decorations;

            Register<MountedPixieWhiteDeed>(new TextDefinition[] { 1074482, 1156915 }, 1156974, 0x2A79, 0, 0, 100, cat);
            Register<MountedPixieLimeDeed>(new TextDefinition[] { 1074482, 1156914 }, 1156974, 0x2A77, 0, 0, 100, cat);
            Register<MountedPixieBlueDeed>(new TextDefinition[] { 1074482, 1156913 }, 1156974, 0x2A75, 0, 0, 100, cat);
            Register<MountedPixieOrangeDeed>(new TextDefinition[] { 1074482, 1156912 }, 1156974, 0x2A73, 0, 0, 100, cat);
            Register<MountedPixieGreenDeed>(new TextDefinition[] { 1074482, 1156911 }, 1156974, 0x2A71, 0, 0, 100, cat);
            Register<UnsettlingPortraitDeed>(1074480, 1156973, 0x2A65, 0, 0, 100, cat);
            Register<CreepyPortraitDeed>(1074481, 1156972, 0x2A69, 0, 0, 100, cat);
            Register<DisturbingPortraitDeed>(1074479, 1156955, 0x2A5D, 0, 0, 100, cat);
            Register<DawnsMusicBox>(1075198, 1156968, 0x2AF9, 0, 0, 100, cat);
            Register<BedOfNailsDeed>(1074801, 1156975, 0, 0x9C8D, 0, 100, cat);
            Register<BrokenCoveredChairDeed>(1076257, 1156950, 0xC17, 0, 0, 100, cat);
            Register<BoilingCauldronDeed>(1076267, 1156949, 0, 0x9CB9, 0, 100, cat);
            Register<SuitOfGoldArmorDeed>(1076265, 1156943, 0x3DAA, 0, 0, 100, cat);
            Register<BrokenBedDeed>(1076263, 1156945, 0, 0x9C8F, 0, 100, cat);
            Register<BrokenArmoireDeed>(1076262, 1156946, 0xC12, 0, 0, 100, cat);
            Register<BrokenVanityDeed>(1076260, 1156947, 0, 0x9C90, 0, 100, cat);
            Register<BrokenBookcaseDeed>(1076258, 1156948, 0xC14, 0, 0, 100, cat);
            Register<SacrificialAltarDeed>(1074818, 1156954, 0, 0x9C8E, 0, 100, cat);
            Register<BrokenChestOfDrawersDeed>(1076261, 1156951, 0xC24, 0, 0, 100, cat);
            Register<StandingBrokenChairDeed>(1076259, 1156952, 0xC1B, 0, 0, 100, cat);
            Register<FountainOfLifeDeed>(1075197, 1156964, 0x2AC0, 0, 0, 100, cat);
            Register<TapestryOfSosaria>(1062917, 1156961, 0x234E, 0, 0, 100, cat);
            Register<RoseOfTrinsic>(1062913, 1156960, 0x234D, 0, 0, 100, cat);
            Register<HearthOfHomeFireDeed>(1062919, 1156958, 0, 0x9C97, 0, 100, cat);

            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1157015 }, 1156916, 0, 0x9CB5, 0, 200, cat, ConstructMiniHouseDeed); // two story wood & plaster
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011317 }, 1156916, 0x22F5, 0, 0, 200, cat, ConstructMiniHouseDeed); // small stone tower
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011307 }, 1156916, 0x22E0, 0, 0, 200, cat, ConstructMiniHouseDeed); // wood and plaster house
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011308 }, 1156916, 0x22E1, 0, 0, 200, cat, ConstructMiniHouseDeed); // thathed-roof cottage
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011312 }, 1156916, 0, 0x9CB2, 0, 200, cat, ConstructMiniHouseDeed); // Tower
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011313 }, 1156916, 0, 0x9CB1, 0, 200, cat, ConstructMiniHouseDeed); // Small stone keep
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011314 }, 1156916, 0, 0x9CB0, 0, 200, cat, ConstructMiniHouseDeed); // Castle

            Register<HangingSwordsDeed>(1076272, 1156936, 0, 0x9C96, 0, 100, cat);
            Register<UnmadeBedDeed>(1076279, 1156935, 0, 0x9C9B, 0, 100, cat);
            Register<CurtainsDeed>(1076280, 1156934, 0, 0x9C93, 0, 100, cat);
            Register<TableWithOrangeClothDeed>(new TextDefinition[] { 1157012, 1157013 }, 1156933, 0x118E, 0, 0, 100, cat);

            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011320 }, 1156916, 0x22F3, 0, 0, 200, cat, ConstructMiniHouseDeed); // sanstone house with patio
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011316 }, 1156916, 0, 0x9CB3, 0, 200, cat, ConstructMiniHouseDeed); // marble house with patio
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011319 }, 1156916, 0x2300, 0, 0, 200, cat, ConstructMiniHouseDeed); // two story villa
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1157014 }, 1156916, 0, 0x9CB6, 0, 200, cat, ConstructMiniHouseDeed); // two story stone & plaster
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011315 }, 1156916, 0, 0x9CB4, 0, 200, cat, ConstructMiniHouseDeed); // Large house with patio
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011309 }, 1156916, 0, 0x9CB7, 0, 200, cat, ConstructMiniHouseDeed); // brick house
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011304 }, 1156916, 0x22C9, 0, 0, 200, cat, ConstructMiniHouseDeed); // field stone house
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011306 }, 1156916, 0x22DF, 0, 0, 200, cat, ConstructMiniHouseDeed); // wooden house
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011305 }, 1156916, 0x22DE, 0, 0, 200, cat, ConstructMiniHouseDeed); // small brick house
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011303 }, 1156916, 0x22E1, 0, 0, 200, cat, ConstructMiniHouseDeed); // stone and plaster house
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011318 }, 1156916, 0x22FB, 0, 0, 200, cat, ConstructMiniHouseDeed); // two-story log cabin
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011321 }, 1156916, 0x22F6, 0, 0, 200, cat, ConstructMiniHouseDeed); // small stone workshop
            Register<MiniHouseDeed>(new TextDefinition[] { 1062096, 1011322 }, 1156916, 0x22F4, 0, 0, 200, cat, ConstructMiniHouseDeed); // small marble workshop

            Register<TableWithBlueClothDeed>(1076276, 1156932, 0x118C, 0, 0, 100, cat);
            Register<CherryBlossomTreeDeed>(1076268, 1156940, 0, 0x9C91, 0, 100, cat);
            Register<IronMaidenDeed>(1076288, 1156924, 0x1249, 0, 0, 100, cat);
            Register<SmallFishingNetDeed>(1076286, 1156923, 0x1EA3, 0, 0, 100, cat);
            Register<StoneStatueDeed>(1076284, 1156922, 0, 0x9C9A, 0, 100, cat);
            Register<WallTorchDeed>(1076282, 1156921, 0x3D98, 0, 0, 100, cat);
            Register<HouseLadderDeed>(1076287, 1156920, 0x2FDE, 0, 0, 100, cat);
            Register<LargeFishingNetDeed>(1076285, 1156919, 0x3D8E, 0, 0, 100, cat);
            Register<FountainDeed>(1076283, 1156918, 0, 0x9C94, 0, 100, cat);
            Register<ScarecrowDeed>(1076608, 1156917, 0x1E34, 0, 0, 100, cat);
            Register<HangingAxesDeed>(1076271, 1156937, 0, 0x9C95, 0, 100, cat);
            Register<AppleTreeDeed>(1076269, 1156938, 0, 0x9C8C, 0, 100, cat);
            Register<GuillotineDeed>(1024656, 1156941, 0x125E, 0, 0, 100, cat);
            Register<SuitOfSilverArmorDeed>(1076266, 1156942, 0x3D86, 0, 0, 100, cat);
            Register<PeachTreeDeed>(1076270, 1156939, 0, 0x9C98, 0, 100, cat);
            Register<CherryBlossomTrunkDeed>(1076784, 1156925, 0x26EE, 0, 0, 100, cat);
            Register<PeachTrunkDeed>(1076786, 1156926, 0xD9C, 0, 0, 100, cat);
            Register<BrokenFallenChairDeed>(1076264, 1156944, 0xC19, 0, 0, 100, cat);
            Register<TableWithRedClothDeed>(1076277, 1156930, 0x118E, 0, 0, 100, cat);
            Register<VanityDeed>(1074027, 1156931, 0, 0x9C9C, 0, 100, cat);
            Register<AppleTrunkDeed>(1076785, 1156927, 0xD98, 0, 0, 100, cat);
            Register<TableWithPurpleClothDeed>(new TextDefinition[] { 1157011, 1157013 }, 1156929, 0x118B, 0, 0, 100, cat);
            Register<WoodenCoffinDeed>(1076274, 1156928, 0, 0x9C92, 0, 100, cat);
            Register<LampPost2>(1071089, 1156650, 0xB22, 0, 0, 200, cat, ConstructLampPost);


            // mounts
            cat = StoreCategory.Mounts;

            Register<ChargerOfTheFallen>(1075187, 1156646, 0x2D9C, 0, 0, 1000, cat);


            // misc
            cat = StoreCategory.Misc;

        }

        public static void Register<T>(TextDefinition name, int tooltip, int itemID, int gumpID, int hue, int cost, StoreCategory cat, Func<Mobile, StoreEntry, Item> constructor = null) where T : Item
        {
            Register(typeof(T), name, tooltip, itemID, gumpID, hue, cost, cat, constructor);
        }

        public static void Register(Type itemType, TextDefinition name, int tooltip, int itemID, int gumpID, int hue, int cost, StoreCategory cat, Func<Mobile, StoreEntry, Item> constructor = null)
        {
            Register(new StoreEntry(itemType, name, tooltip, itemID, gumpID, hue, cost, cat, constructor));
        }

        public static void Register<T>(TextDefinition[] name, int tooltip, int itemID, int gumpID, int hue, int cost, StoreCategory cat, Func<Mobile, StoreEntry, Item> constructor = null) where T : Item
        {
            Register(typeof(T), name, tooltip, itemID, gumpID, hue, cost, cat, constructor);
        }

        public static void Register(Type itemType, TextDefinition[] name, int tooltip, int itemID, int gumpID, int hue, int cost, StoreCategory cat, Func<Mobile, StoreEntry, Item> constructor = null)
        {
            Register(new StoreEntry(itemType, name, tooltip, itemID, gumpID, hue, cost, cat, constructor));
        }

        public static StoreEntry GetEntry(Type t)
        {
            return Entries.FirstOrDefault(e => e.ItemType == t);
        }

        public static void Register(StoreEntry entry)
        {
            Entries.Add(entry);
        }

        public static bool CanSearch(Mobile m)
        {
            return m != null && m.Region.GetLogoutDelay(m) <= TimeSpan.Zero;
        }

        public static void UOStoreRequest(NetState state, PacketReader pvSrc)
        {
            OpenStore(state.Mobile as PlayerMobile);
        }

        public static void OpenStore(PlayerMobile user, StoreEntry forcedEntry = null)
        {
            if (user == null || user.NetState == null)
            {
                return;
            }

            if (!Enabled)
            {
                // The promo code redemption system is currently unavailable. Please try again later.
                user.SendLocalizedMessage(1062904);
                return;
            }

            if (Configuration.CurrencyImpl == CurrencyType.None)
            {
                // The promo code redemption system is currently unavailable. Please try again later.
                user.SendLocalizedMessage(1062904);
                return;
            }

            if (user.AccessLevel < AccessLevel.Counselor && !CanSearch(user))
            {
                // Before using the in game store, you must be in a safe log-out location
                // such as an inn or a house which has you on its Owner, Co-owner, or Friends list.
                user.SendLocalizedMessage(1156586);
                return;
            }

            if (!user.HasGump(typeof(UltimaStoreGump)))
            {
                BaseGump.SendGump(new UltimaStoreGump(user, forcedEntry));
            }
        }

		#region Constructors
		public static Item ConstructPigments(Mobile m, StoreEntry entry)
        {
            PigmentType type = PigmentType.None;

            for (int i = 0; i < PigmentsOfTokuno.Table.Length; i++)
            {
                if (PigmentsOfTokuno.Table[i][1] == entry.Name[1].Number)
                {
                    type = (PigmentType)i;
                    break;
                }
            }

            if (type != PigmentType.None)
            {
                return new PigmentsOfTokuno(type, 50);
            }

            return null;
        }

        public static Item ConstructEarrings(Mobile m, StoreEntry entry)
        {
            AosElementAttribute ele = AosElementAttribute.Physical;

            switch (entry.Name[0].Number)
            {
                case 1071092: ele = AosElementAttribute.Fire; break;
                case 1071093: ele = AosElementAttribute.Cold; break;
                case 1071094: ele = AosElementAttribute.Poison; break;
                case 1071095: ele = AosElementAttribute.Energy; break;
            }

            return new EarringsOfProtection(ele);
        }

        public static Item ConstructMiniHouseDeed(Mobile m, StoreEntry entry)
        {
            int label = entry.Name[1].Number;

            switch (label)
            {
                default:
                    for (int i = 0; i < MiniHouseInfo.Info.Length; i++)
                    {
                        if (MiniHouseInfo.Info[i].LabelNumber == entry.Name[1].Number)
                        {
                            MiniHouseType type = (MiniHouseType)i;

                            return new MiniHouseDeed(type);
                        }
                    }
                    return null;
                case 1157015: return new MiniHouseDeed(MiniHouseType.TwoStoryWoodAndPlaster);
                case 1157014: return new MiniHouseDeed(MiniHouseType.TwoStoryStoneAndPlaster);
            }
        }

        public static Item ConstructLampPost(Mobile m, StoreEntry entry)
        {
            LampPost2 item = new LampPost2
            {
                Movable = true,
                LootType = LootType.Blessed
            };

            return item;
        }
        #endregion

        public static void AddPendingItem(Mobile m, Item item)
		{
			List<Item> list;

			if (!PendingItems.TryGetValue(m, out list))
            {
                PendingItems[m] = list = new List<Item>();
            }

            if (!list.Contains(item))
            {
                list.Add(item);
            }

            UltimaStoreContainer.DropItem(item);
        }

        public static bool HasPendingItem(PlayerMobile pm)
        {
            return PendingItems.ContainsKey(pm);
        }

        public static void CheckPendingItem(Mobile m)
		{
			List<Item> list;

			if (PendingItems.TryGetValue(m, out list))
            {
                int index = list.Count;

                while (--index >= 0)
                {
                    if (index >= list.Count)
                    {
                        continue;
                    }

                    Item item = list[index];

                    if (item != null)
                    {
                        if (m.Backpack != null && m.Alive && m.Backpack.TryDropItem(m, item, false))
                        {
							if (item.LabelNumber > 0 || item.Name != null)
                            {
                                string name = item.LabelNumber > 0 ? ("#" + item.LabelNumber) : item.Name;

                                // Your purchase of ~1_ITEM~ has been placed in your backpack.
                                m.SendLocalizedMessage(1156844, name);
                            }
                            else
                            {
                                // Your purchased item has been placed in your backpack.
                                m.SendLocalizedMessage(1156843);
                            }

                            list.RemoveAt(index);
                        }
                    }
                    else
                    {
                        list.RemoveAt(index);
                    }
                }

                if (list.Count == 0 && PendingItems.Remove(m))
                {
                    list.TrimExcess();
                }
            }
        }

        public static List<StoreEntry> GetSortedList(string searchString)
        {
            List<StoreEntry> list = new List<StoreEntry>();

            list.AddRange(Entries.Where(e => Insensitive.Contains(GetStringName(e.Name), searchString)));

            return list;
        }

        public static string GetStringName(TextDefinition[] text)
        {
            string str = string.Empty;

            foreach (TextDefinition td in text)
            {
                if (td.Number > 0 && StringList.Localization != null)
                {
                    str += string.Format("{0} ", StringList.Localization.GetString(td.Number));
                }
                else if (!string.IsNullOrWhiteSpace(td.String))
                {
                    str += string.Format("{0} ", td.String);
                }
            }

            return str;
        }

        public static string GetStringName(TextDefinition text)
        {
            string str = text.String;

            return str ?? string.Empty;
        }

        public static List<StoreEntry> GetList(StoreCategory cat, StoreEntry forcedEntry = null)
        {
            if (forcedEntry != null)
            {
                return new List<StoreEntry>() { forcedEntry };
            }

            return Entries.Where(e => e.Category == cat).ToList();
        }

        public static void SortList(List<StoreEntry> list, SortBy sort)
        {
            switch (sort)
            {
                case SortBy.Name:
                    list.Sort((a, b) => string.CompareOrdinal(GetStringName(a.Name), GetStringName(b.Name)));
                    break;
                case SortBy.PriceLower:
                    list.Sort((a, b) => a.Price.CompareTo(b.Price));
                    break;
                case SortBy.PriceHigher:
                    list.Sort((a, b) => b.Price.CompareTo(a.Price));
                    break;
                case SortBy.Newest:
                    break;
                case SortBy.Oldest:
                    list.Reverse();
                    break;
            }
        }

        public static int CartCount(Mobile m)
        {
            PlayerProfile profile = GetProfile(m, false);

            if (profile != null)
            {
                return profile.Cart.Count;
            }

            return 0;
        }

        public static int GetSubTotal(Dictionary<StoreEntry, int> cart)
        {
            if (cart == null || cart.Count == 0)
            {
                return 0;
            }

            double sub = 0.0;

            foreach (KeyValuePair<StoreEntry, int> kvp in cart)
            {
                sub += kvp.Key.Cost * kvp.Value;
            }

            return (int)sub;
        }

        public static int GetCurrency(Mobile m, bool sendMessage = false)
        {
            switch (Configuration.CurrencyImpl)
            {
                case CurrencyType.Sovereigns:
                    {
                        if (m is PlayerMobile)
                        {
                            return ((PlayerMobile)m).AccountSovereigns;
                        }
                    }
                    break;
                case CurrencyType.Gold:
                    return Banker.GetBalance(m);
                case CurrencyType.Custom:
                    return Configuration.GetCustomCurrency(m);
            }

            return 0;
        }

        public static void TryPurchase(Mobile m)
        {
            Dictionary<StoreEntry, int> cart = GetCart(m);
            int total = GetSubTotal(cart);

            if (cart == null || cart.Count == 0 || total == 0)
            {
                // Purchase failed due to your cart being empty.
                m.SendLocalizedMessage(1156842);
            }
            else if (total > GetCurrency(m, true))
            {
                if (m is PlayerMobile)
                {
					BaseGump.SendGump(new NoFundsGump((PlayerMobile)m));
                }
            }
            else
            {
                int subtotal = 0;
                bool fail = false;

                List<StoreEntry> remove = new List<StoreEntry>();

                foreach (KeyValuePair<StoreEntry, int> entry in cart)
                {
                    for (int i = 0; i < entry.Value; i++)
                    {
                        if (!entry.Key.Construct(m))
                        {
                            fail = true;

                            try
                            {
								using (StreamWriter op = File.AppendText("UltimaStoreError.log"))
								{
									op.WriteLine("Bad Constructor: {0}", entry.Key.ItemType.Name);

									Utility.PushColor(ConsoleColor.Red);
									Console.WriteLine("[Ultima Store]: Bad Constructor: {0}", entry.Key.ItemType.Name);
									Utility.PopColor();
								}
                            }
                            catch (Exception e)
							{
								Utility.PushColor(ConsoleColor.Red);
								Console.WriteLine("[Ultima Store]: {0}", e);
								Utility.PopColor();
							}
                        }
                        else
                        {
                            remove.Add(entry.Key);

                            subtotal += entry.Key.Cost;
                        }
                    }
                }

                if (subtotal > 0)
                {
                    DeductCurrency(m, subtotal);
                }

                PlayerProfile profile = GetProfile(m);

                foreach (StoreEntry entry in remove)
                {
                    profile.RemoveFromCart(entry);
                }

                if (fail)
                {
                    // Failed to process one of your items. Please check your cart and try again.
                    m.SendLocalizedMessage(1156853);
                }
            }
        }

        /// <summary>
        /// Should have already passed GetCurrency
        /// </summary>
        /// <param name="m"></param>
        /// <param name="amount"></param>
        public static int DeductCurrency(Mobile m, int amount)
        {
            switch (Configuration.CurrencyImpl)
            {
                case CurrencyType.Sovereigns:
                    {
                        if (m is PlayerMobile && ((PlayerMobile)m).WithdrawSovereigns(amount))
                        {
                            return amount;
                        }
                    }
                    break;
                case CurrencyType.Gold:
                    {
                        if (Banker.Withdraw(m, amount))
                        {
                            return amount;
                        }
                    }
                    break;
                case CurrencyType.Custom:
                    return Configuration.DeductCustomCurrecy(m, amount);
            }

            return 0;
        }

        #region Player Persistence
        public static Dictionary<Mobile, PlayerProfile> PlayerProfiles { get; private set; }

        public static PlayerProfile GetProfile(Mobile m, bool create = true)
        {
			PlayerProfile profile;

			if ((!PlayerProfiles.TryGetValue(m, out profile) || profile == null) && create)
            {
                PlayerProfiles[m] = profile = new PlayerProfile(m);
            }

            return profile;
        }

        public static Dictionary<StoreEntry, int> GetCart(Mobile m)
        {
            PlayerProfile profile = GetProfile(m, false);

            if (profile != null)
            {
                return profile.Cart;
            }

            return null;
        }

        public static void OnSave(WorldSaveEventArgs e)
        {
            Persistence.Serialize(FilePath, Serialize);
        }

        public static void OnLoad()
        {
            Persistence.Deserialize(FilePath, Deserialize);
        }

        private static void Serialize(GenericWriter writer)
        {
            writer.Write(0);

            writer.Write(_UltimaStoreContainer);

            writer.Write(PendingItems.Count);

            foreach (KeyValuePair<Mobile, List<Item>> kvp in PendingItems)
            {
                writer.Write(kvp.Key);
                writer.WriteItemList(kvp.Value, true);
            }

            writer.Write(PlayerProfiles.Count);

            foreach (KeyValuePair<Mobile, PlayerProfile> pe in PlayerProfiles)
            {
                pe.Value.Serialize(writer);
            }
        }

        private static void Deserialize(GenericReader reader)
        {
            reader.ReadInt();

            _UltimaStoreContainer = reader.ReadItem<UltimaStoreContainer>();

            int count = reader.ReadInt();

            for (int i = 0; i < count; i++)
            {
                Mobile m = reader.ReadMobile();
                List<Item> list = reader.ReadStrongItemList<Item>();

                if (m != null && list.Count > 0)
                {
                    PendingItems[m] = list;
                }
            }

            count = reader.ReadInt();

            for (int i = 0; i < count; i++)
            {
                PlayerProfile pe = new PlayerProfile(reader);

                if (pe.Player != null)
                {
                    PlayerProfiles[pe.Player] = pe;
                }
            }
        }
        #endregion
    }

    public sealed class UltimaStoreContainer : Container
    {
        private static readonly List<Item> _DisplayItems = new List<Item>();

        public override bool Decays { get { return false; } }

        public override string DefaultName { get { return "Ultima Store Display Container"; } }

        public UltimaStoreContainer()
            : base(0) // No Draw
        {
            Movable = false;
            Visible = false;

            Internalize();
        }

        public UltimaStoreContainer(Serial serial)
            : base(serial)
        { }

        public void AddDisplayItem(Item item)
        {
            if (item == null)
            {
                return;
            }

            if (!_DisplayItems.Contains(item))
            {
                _DisplayItems.Add(item);
            }

            DropItem(item);
        }

        public Item FindDisplayItem(Type t)
        {
            Item item = GetDisplayItem(t);

            if (item == null)
            {
                item = Loot.Construct(t);

                if (item != null)
                {
                    AddDisplayItem(item);
                }
            }

            return item;
        }

        public Item GetDisplayItem(Type t)
        {
            return _DisplayItems.FirstOrDefault(x => x.GetType() == t);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);

            writer.WriteItemList(_DisplayItems, true);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            reader.ReadInt();

            List<Item> list = reader.ReadStrongItemList();

            if (list.Count > 0)
            {
                Timer.DelayCall(o => o.ForEach(AddDisplayItem), list);
            }
        }
    }
}
