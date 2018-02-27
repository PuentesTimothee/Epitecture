using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Imgur.API;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;
using System.IO;
using System.Collections.ObjectModel;

namespace epitecture
{
    public class ImgurAPI
    {
        #region Variables
        public ObservableCollection<Picture> CurrentSearch { get; private set; }

        private string ClientId { get; set; }
        private string ClientSecret { get; set; }
        private ImgurClient client { get; set; }
        #endregion

        #region Methods
        public ImgurAPI(string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            client = new ImgurClient(clientId, clientSecret);
            CurrentSearch = new ObservableCollection<Picture>();
        }

        public async Task GetImage()
        {
            try
            {
                var endpoint = new ImageEndpoint(client);
                var image = await endpoint.GetImageAsync("  ");

                CurrentSearch.Clear();
                CurrentSearch.Add(fillPicture(image));
                Debug.Write("Image retrieved. Image Url: " + image.Link);
            }
            catch (ImgurException imgurEx)
            {
                Debug.Write("An error occurred getting an image from Imgur.");
                Debug.Write(imgurEx.Message);
            }
        }

        public async Task GetGalleryTagAsync(string tagName)
        {
            var endpoint = new GalleryEndpoint(client);
            var tag = await endpoint.GetGalleryTagAsync(tagName);

            CurrentSearch.Clear();
            foreach (IGalleryItem item in tag.Items)
            {
                if (item is IGalleryImage)
                    CurrentSearch.Add(fillPicture(item as IGalleryImage));
                else if (item is IGalleryAlbum)
                {
                    if ((item as IGalleryAlbum).Images.First() != null)
                        CurrentSearch.Add(fillPicture((item as IGalleryAlbum).Images.First()));
                }
            }
        }

        public async Task GetRandomGalleryAsync()
        {
            var endpoint = new GalleryEndpoint(client);
            var images = await endpoint.GetRandomGalleryAsync();

            CurrentSearch.Clear();
            foreach (IGalleryItem item in images)
            {
                if (item is IGalleryImage)
                    CurrentSearch.Add(fillPicture(item as IGalleryImage));
                else if (item is IGalleryAlbum)
                {
                    if ((item as IGalleryAlbum).Images.First() != null)
                        CurrentSearch.Add(fillPicture((item as IGalleryAlbum).Images.First()));
                }
            }
        }

        public async Task SearchGalleryAsync(string query)
        {
            var endpoint = new GalleryEndpoint(client);
            var images = await endpoint.SearchGalleryAsync(query);

            CurrentSearch.Clear();
            foreach (IGalleryItem item in images)
            {
                if (item is IGalleryImage)
                    CurrentSearch.Add(fillPicture(item as IGalleryImage));
                else if (item is IGalleryAlbum)
                {
                    if ((item as IGalleryAlbum).Images.First() != null)
                        CurrentSearch.Add(fillPicture((item as IGalleryAlbum).Images.First()));
                }
            }
        }

        public async Task<Picture> UploadImage(Stream fs)
        {
            try
            {
                var endpoint = new ImageEndpoint(client);
                IImage image;
                image = await endpoint.UploadImageStreamAsync(fs);
                Debug.Write("Image uploaded. Image Url: " + image.Link + "\n");
                return fillPicture(image);
            }
            catch (ImgurException imgurEx)
            {
                Debug.WriteLine("An error occurred updating an image to Imgur.");
                Debug.WriteLine(imgurEx.Message);
            }

            return null;
        }

        public async Task<bool> UpdateImage(Picture pic)
        {
            try
            {
                if (pic == null)
                    return false;
                var endpoint = new ImageEndpoint(client);
                var updated = await endpoint.UpdateImageAsync(pic.UploadedId, pic.Title, pic.Description);
                return updated;
            }
            catch (ImgurException imgurEx)
            {
                Debug.WriteLine("An error occurred updating an image to Imgur.");
                Debug.WriteLine(imgurEx.Message);
            }
            return false;
        }

        #region Fill Methods
        private Picture fillPicture(IGalleryImage image)
        {
            Picture pic = new Picture();

            pic.Link = image.Link;
            pic.Name = ((image.Topic != null) ? (image.Topic) : (""));
            pic.Title = ((image.Title != null) ? (image.Title) : (""));

            return pic;
        }

        private Picture fillPicture(IImage image)
        {
            Picture pic = new Picture();

            pic.UploadedId = image.DeleteHash;
            pic.Link = image.Link;
            pic.Name = ((image.Name != null) ? (image.Name) : (""));
            pic.Title = ((image.Title != null) ? (image.Title) : (""));
            pic.Description = ((image.Description != null) ? (image.Description) : (""));

            return pic;
        }
        #endregion
     
        #endregion
    }
}
