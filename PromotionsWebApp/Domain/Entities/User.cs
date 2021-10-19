using Microsoft.AspNetCore.Identity;
using PromotionsWebApp.Domain.Abstract;
using PromotionsWebApp.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PromotionsWebApp.Domain.Entities
{
    public class User:IdentityUser
    {
        public User(TitleEnum title,string name,string surname,UserRoleEnum role,
                    string email)
        {
            Title = title;
            FirstName = name;
            LastName = surname;
            Role = role;
            Email = email;
            UserName = FirstName + " " + LastName;
            ProfileImage = GenerateAvatarImage();
        }

        public TitleEnum Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] ProfileImage { get; set; }
        public UserRoleEnum Role { get; set; }
        public bool PasswordReset { get; set; }
        public bool isDeleted { get; set; }
        public virtual IEnumerable<UserJob> Jobs { get; set; }
        public virtual IEnumerable<Qualification> Qualifications { get; set; }
        private byte[] GenerateAvatarImage()
        {
            //first, create a dummy bitmap just to get a graphics object  
            string text = FirstName.Split(' ').Select(s => s[0]).ToString() + LastName.Split(' ').Select(s => s[0]).ToString();
            System.Drawing.Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);
            Font font = new Font(FontFamily.GenericSerif, 45, FontStyle.Bold);
            Color textColor = ColorTranslator.FromHtml("#FFF");
            Color backColor = ColorTranslator.FromHtml("#83B869");
            //measure the string to see how big the image needs to be  
            SizeF textSize = drawing.MeasureString(text, font);

            //free up the dummy image and old graphics object  
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size  
            img = new Bitmap(110, 110);

            drawing = Graphics.FromImage(img);

            //paint the background  
            drawing.Clear(backColor);

            //create a brush for the text  
            Brush textBrush = new SolidBrush(textColor);

            //drawing.DrawString(text, font, textBrush, 0, 0);  
            drawing.DrawString(text, font, textBrush, new Rectangle(-2, 20, 200, 110));

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();
            using (var ms = new MemoryStream())
            {
                img.Save(ms, img.RawFormat);
                return ms.ToArray();
            }
        }
    }
}
