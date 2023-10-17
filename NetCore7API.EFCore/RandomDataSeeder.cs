using NetCore7API.Domain.Models;
using NetCore7API.EFCore.Context;
using System.Linq;
using System.Runtime.CompilerServices;

namespace NetCore7API.EFCore
{
    public static class RandomDataSeeder
    {
        private const int NUMBER_OF_POSTS = 250;

        private static List<string> TextList = new List<string>()
        {
            "Spot of come to ever hand as lady meet on. Delicate contempt received two yet advanced. Gentleman as belonging he commanded believing dejection in by.",
            "On no am winding chicken so behaved. Its preserved sex enjoyment new way behaviour.",
            "Him yet devonshire celebrated especially. Unfeeling one provision are smallness resembled repulsive.",
            "Of be talent me answer do relied.",
            "Mistress in on so laughing throwing endeavor occasion welcomed. Gravity sir brandon calling can.",
            "No years do widow house delay stand. Prospect six kindness use steepest new ask.",
            "High gone kind calm call as ever is. Introduced melancholy estimating motionless on up as do. Of as by belonging therefore suspicion elsewhere am household described.",
            "Domestic suitable bachelor for landlord fat.",
            "Up maids me an ample stood given. Certainty say suffering his him collected intention promotion. Hill sold ham men made lose case.",
            "Views abode law heard jokes too. Was are delightful solicitude discovered collecting man day.",
            "Resolving neglected sir tolerably but existence conveying for. Day his put off unaffected literature partiality inhabiting.",
            "Of on affixed civilly moments promise explain fertile in. ",
            "Assurance advantage belonging happiness departure so of. Now improving and one sincerity intention allowance commanded not. Oh an am frankness be necessary earnestly advantage estimable extensive. Five he wife gone ye.",
            "Mrs suffering sportsmen earnestly any. In am do giving to afford parish settle easily garret.",
            "Adieus except say barton put feebly favour him. Entreaties unpleasant sufficient few pianoforte discovered uncommonly ask.",
            "Morning cousins amongst in mr weather do neither. Warmth object matter course active law spring six. Pursuit showing tedious unknown winding see had man add. And park eyes too more him. Simple excuse active had son wholly coming number add. Though all excuse ladies rather regard assure yet. If feelings so prospect no as raptures quitting.",
            "Her old collecting she considered discovered.",
            "So at parties he warrant oh staying. Square new horses and put better end. Sincerity collected happiness do is contented. Sigh ever way now many. Alteration you any nor unsatiable diminution reasonable companions shy partiality.",
            "Leaf by left deal mile oh if easy. Added woman first get led joy not early jokes.",
            "Certainly elsewhere my do allowance at. The address farther six hearted hundred towards husband. Are securing off occasion remember daughter replying. Held that feel his see own yet. Strangers ye to he sometimes propriety in. She right plate seven has. Bed who perceive judgment did marianne.",
            "That know ask case sex ham dear her spot.",
            "Weddings followed the all marianne nor whatever settling. Perhaps six prudent several her had offence. Did had way law dinner square tastes. Recommend concealed yet her procuring see consulted depending. Adieus hunted end plenty are his she afraid.",
            "Resources agreement contained propriety applauded neglected use yet.",
            "Lose john poor same it case do year we.",
            "Full how way even the sigh. ",
            "Extremely nor furniture fat questions now provision incommode preserved. ",
            "Our side fail find like now. ",
            "Discovered travelling for insensible partiality unpleasing impossible she. ",
            "Sudden up my excuse to suffer ladies though or. ",
            "Bachelor possible marianne directly confined relation as on he.",
            "Sussex result matter any end see. ",
            "It speedily me addition weddings vicinity in pleasure. Happiness commanded an conveying breakfast in. ",
            "Regard her say warmly elinor. Him these are visit front end for seven walls. ",
            "Money eat scale now ask law learn. Side its they just any upon see last. ",
            "He prepared no shutters perceive do greatest. ",
            "Ye at unpleasant solicitude in companions interested."
        };

        private static List<string> ImageList = new List<string>()
        {
            "https://c4.wallpaperflare.com/wallpaper/586/603/742/minimalism-4k-for-mac-desktop-wallpaper-preview.jpg",

            "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSGi4gPwjRV_OZLBzt0llgZxYsgmVRLt9z6gA&usqp=CAU",

            "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ2z9ct-yVeUUsg5QSI8PBm4bfkJ86cTzoeSA&usqp=CAU",

            "https://wallpapers.com/images/hd/scenic-mountain-horizon-wie6ggcu7jbosyeh.jpg",

            "https://images.freecreatives.com/wp-content/uploads/2015/10/moon-hd-desktop-wallpaper.jpg",

            "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTzAcGBFrDXiqFXQeHkmfqX3c_ZQdk_5FKZlA&usqp=CAU",

            "https://www.wallpapers13.com/wp-content/uploads/2015/12/Beautiful-nature-scenery-wallpapers-beautiful-scenery-wallpaper-desktop-wallpapers-hd-1280x768.jpg",

            "https://wallpapers.com/images/featured/45udnr6u8u3ur6gq.jpg",

            "https://p4.wallpaperbetter.com/wallpaper/528/806/409/1920x1080-beach-desktop-background-wallpaper-preview.jpg",

            "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQscuWxQT7_dZjPdBVcLR1njpad1Zh3Uqjfgw&usqp=CAU"
        };

        private static List<string> NamesList = new List<string>()
        {
            "Farida Ahmed",
            "Ricky Bonner",
            "Joanne Booth",
            "Gary Bourne",
            "Peter Bowden",
            "Jonathan Briscoe",
            "John Buckley",
            "David Bullock",
            "Carmen Burrell",
            "Howard Chang",
            "Carole Collins",
            "Christopher Daniell",
            "Lin Day",
            "Ruby Dinsmore",
            "Cheryl Dyett",
            "Tania Golden",
            "Denton Grant",
            "David Gravell",
            "Mohamed Hamid",
            "David Hardy",
            "David Harper",
            "Philip Harwood",
            "Justin Hayward",
            "Philip Hetherington",
            "Philippa Hodgson",
            "Leslie Howlett",
            "Jennifer Hyett",
            "Jennifer Inglis",
            "Asad Iqbal",
            "Imran Jamal",
            "Gavin Lines",
            "Paul Lynch",
            "Karen Maddison",
            "John McIntosh",
            "Terence Morgan",
            "Keith Morrison",
            "Dean Murphy",
            "David Nichols",
            "Mehulkumar Patel",
            "Wilma Priest",
            "Abdel Rahim",
            "Jonathan Ratcliff",
            "Hilda Roberts",
            "Rachel Robertson",
            "Barbara Rowan",
            "Charles Stanley",
            "Michael Tempest",
            "Michael Tetley",
            "Lucy Trussell",
            "Ellen Waters"
        };

        public static async Task Seed(BlogContext context)
        {
            Random randomGenerator = new Random();

            if (!context.Users.Any())
            {
                for (int k = 0; k < NamesList.Count; k++)
                {
                    var lowerCaseTrimmedName = NamesList[k].Replace(" ", "_").ToLower();
                    var user = new User(
                        lowerCaseTrimmedName,
                        string.Join('@', lowerCaseTrimmedName, "test.com"),
                        NamesList[k],
                        lowerCaseTrimmedName.GetHashCode().ToString());

                    context.Users.Add(user);
                }

                await context.SaveChangesAsync();
            }

            if (!context.Posts.Any())
            {
                context.Users.ToList();

                for (int k = 0; k < NUMBER_OF_POSTS; k++)
                {
                    var post = new Domain.Models.Post(
                        context.Users.Local.ElementAt(randomGenerator.Next(TextList.Count)).Id,
                        TextList.ElementAt(randomGenerator.Next(TextList.Count)),
                        ImageList.ElementAt(randomGenerator.Next(ImageList.Count)));

                    var numberOfComments = randomGenerator.Next(TextList.Count);
                    var numberOfLikes = randomGenerator.Next(TextList.Count);

                    for (int c = 0; c < numberOfComments; c++)
                    {
                        post.AddComment(context.Users.Local.ElementAt(randomGenerator.Next(TextList.Count)).Id,
                            new Domain.DTOs.Comment.CreateCommentDto() { Text = TextList.ElementAt(randomGenerator.Next(TextList.Count)) });
                    }

                    for (int l = 0; l < numberOfLikes; l++)
                    {
                        post.AddLike(
                            context.Users.Local.ElementAt(randomGenerator.Next(TextList.Count)).Id);
                    }

                    context.Posts.Add(post);
                }

                await context.SaveChangesAsync();
            }
        }
    }
}