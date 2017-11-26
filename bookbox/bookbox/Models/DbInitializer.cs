using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookBox.Models
{
    public class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!userManager.Users.Any())
            {
                IdentityUser admin = new IdentityUser() { UserName = "Admin" };
                await userManager.CreateAsync(admin, "Admin1234");
                await userManager.AddToRoleAsync(admin, "Admin");

                IdentityUser user = new IdentityUser() { UserName = "User" };
                await userManager.CreateAsync(user, "User1234");
                await userManager.AddToRoleAsync(user, "User");
            }

            if (!context.Authors.Any())
            {
                context.Authors.AddRange(Authors.Select(c => c.Value));
            }

            if (!context.Books.Any())
            {
                context.AddRange
                (
                    new Book { Title = "Harry Potter and the Philosopher's Stone", ISBN = "0747532699", ReleaseDate = new DateTime(1997, 06, 30), AveragedRating = 0, PicturePath = "/images/0747532699.jpg", Author = Authors["Joanne Kathleen Rowling"], Description = "Harry Potter and the Philosopher's Stone is a fantasy novel written by British author J. K. Rowling. It is the first novel in the Harry Potter series and Rowling's debut novel, first published in 1997 by Bloomsbury. It was published in the United States as Harry Potter and the Sorcerer's Stone by Scholastic Corporation in 1998. The plot follows Harry Potter, a young wizard who discovers his magical heritage as he makes close friends and a few enemies in his first year at the Hogwarts School of Witchcraft and Wizardry. With the help of his friends, Harry faces an attempted comeback by the dark wizard Lord Voldemort, who killed Harry's parents, but failed to kill Harry when he was just 15 months old." },
                    new Book { Title = "Harry Potter and the Chamber of Secrets", ISBN = "0747538492", ReleaseDate = new DateTime(1998, 07, 02), AveragedRating = 0, PicturePath = "/images/0747538492.jpg", Author = Authors["Joanne Kathleen Rowling"], Description = "Harry Potter and the Chamber of Secrets is a fantasy novel written by British author J. K. Rowling and the second novel in the Harry Potter series. The plot follows Harry's second year at Hogwarts School of Witchcraft and Wizardry, during which a series of messages on the walls of the school's corridors warn that the Chamber of Secrets has been opened and that the heir of Slytherin would kill all pupils who do not come from all-magical families. These threats are found after attacks which leave residents of the school petrified (frozen like stone). Throughout the year, Harry and his friends Ron and Hermione investigate the attacks." },
                    new Book { Title = "Harry Potter and the Prisoner of Azkaban", ISBN = "0747542155", ReleaseDate = new DateTime(1999, 07, 08), AveragedRating = 0, PicturePath = "/images/0747542155.jpg", Author = Authors["Joanne Kathleen Rowling"], Description = "Harry Potter and the Prisoner of Azkaban is a fantasy novel written by British author J. K. Rowling and the third novel in the Harry Potter series. The book follows Harry Potter, a young wizard, in his third year at Hogwarts School of Witchcraft and Wizardry. Along with friends Ronald Weasley and Hermione Granger, Harry investigates Sirius Black, an escaped prisoner from Azkaban who they believe is one of Lord Voldemort's old allies." },

                    new Book { Title = "Carrie", ISBN = "9780385086950", ReleaseDate = new DateTime(1974, 04, 05), AveragedRating = 0, PicturePath = "/images/9780385086950.jpg", Author = Authors["Stephen King"], Description = "Carrie is a novel by American author Stephen King. It was his first published novel, released on April 5, 1974, with an approximate first print-run of 30,000 copies.[1] Set primarily in the then-future year of 1979, it revolves around the eponymous Carrie White, a misfit and bullied high school girl who uses her newly discovered telekinetic powers to exact revenge on those who torment her. While in this process, she causes one of the worst local disasters in American history. King has commented that he finds the work to be raw and with a surprising power to hurt and horrify. It is one of the most frequently banned books in United States schools.[2] Much of the book uses newspaper clippings, magazine articles, letters, and excerpts from books to tell how Carrie destroyed the fictional town of Chamberlain, Maine while exacting revenge on her sadistic classmates and her own mother Margaret." },
                    new Book { Title = "The Shining", ISBN = "9780385121675", ReleaseDate = new DateTime(1977, 01, 28), AveragedRating = 0, PicturePath = "/images/9780385121675.jpg", Author = Authors["Stephen King"], Description = "The Shining centers on the life of Jack Torrance, an aspiring writer and recovering alcoholic who accepts a position as the off-season caretaker of the historic Overlook Hotel in the Colorado Rockies. His family accompanies him on this job, including his young son Danny Torrance, who possesses the shining, an array of psychic abilities that allow Danny to see the hotel's horrific past. Soon, after a winter storm leaves them snowbound, the supernatural forces inhabiting the hotel influence Jack's sanity, leaving his wife and son in incredible danger." },
                    new Book { Title = "It", ISBN = "0670813028", ReleaseDate = new DateTime(1986, 09, 15), AveragedRating = 0, PicturePath = "/images/0670813028.jpg", Author = Authors["Stephen King"], Description = "It is a 1986 horror novel by American author Stephen King. It was his 22nd book and 18th novel written under his own name. The story follows the experiences of seven children as they are terrorized by an entity that exploits the fears and phobias of its victims to disguise itself while hunting its prey. It primarily appears in the form of a clown to attract its preferred prey of young children. The novel is told through narratives alternating between two time periods, and is largely told in the third-person omniscient mode. It deals with themes that eventually became King staples: the power of memory, childhood trauma and its recurrent echoes in adulthood, the ugliness lurking behind a façade of small-town quaintness, and overcoming evil through mutual trust and sacrifice." },

                    new Book { Title = "American goods", ISBN = "0380973650", ReleaseDate = new DateTime(2001, 06, 19), AveragedRating = 0, PicturePath = "/images/0380973650.jpg", Author = Authors["Neil Richard Gaiman"], Description = "The premise of the novel is that gods and mythological creatures exist because people believe in them (a type of thoughtform). Immigrants to the United States brought with them spirits and gods. The power of these mythological beings has diminished as people's beliefs waned. New gods have arisen, reflecting the American obsessions with media, celebrity, technology and drugs, among other things." }
                );
            }

            context.SaveChanges();
        }

        private static Dictionary<string, Author> authors;
        public static Dictionary<string, Author> Authors
        {
            get
            {
                if (authors == null)
                {
                    var authorsList = new Author[]
                    {
                        new Author { Name = "Joanne Kathleen", LastName = "Rowling" },
                        new Author { Name = "Neil Richard", LastName = "Gaiman" },
                        new Author { Name = "Stephen", LastName = "King" },
                    };

                    authors = new Dictionary<string, Author>();

                    foreach (Author author in authorsList)
                    {
                        authors.Add(author.Name + " " + author.LastName, author);
                    }
                }

                return authors;
            }
        }
    }
}
