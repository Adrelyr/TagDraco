using System;
using System.Diagnostics;
using System.Drawing;

namespace TagDraco.Core
{
    public class Tags : IDisposable
    {
        public string   Album               { get; set; } = string.Empty;
        public string   Title               { get; set; } = string.Empty;
        public string[] Artists             { get; set; } = new string[0];
        public string[] ContributingArtists { get; set; } = new string[0];
        public uint     Year                { get; set; } = 2000;
        public uint     Track               { get; set; } = 0;
        public string[] Genres              { get; set; } = new string[0];
        public Image    AlbumCover          { get; set; } = null;
        public string   FilePath            { get; set; } = string.Empty;

        public Tags()
        {

        }

        ~Tags()
        {
            AlbumCover.Dispose();
            Debug.WriteLine("Tag has been yeeted");
        }

        public Tags(string Album, string Title, string[] Artists, string[] ContributingArtists, uint Year, uint Track, string[] Genres, Image AlbumCover, string FilePath)
        {
            this.Album = Album;
            this.Title = Title;
            this.Genres = Genres;
            this.Year = Year;
            this.Artists = Artists;
            this.ContributingArtists = ContributingArtists;
            this.Track = Track;
            this.AlbumCover = AlbumCover;
            this.FilePath = FilePath;
        }

        public string GetJoinedArtists()
        {
             return Join(Artists);
        }

        public string GetJoinedContributingArtists()
        {
            return Join(ContributingArtists);
        }

        public string GetJoinedGenres()
        {
            return Join(Genres);
        }

        public string Join(string[] toJoin)
        {
            string joined = string.Empty;
            for (int i = 0; i < toJoin.Length; i++)
            {
                if (i == toJoin.Length - 1)
                {
                    joined += toJoin[i];
                    continue;
                }
                joined += toJoin[i] + ",";
            }
            return joined;
        }

        public void Dispose()
        {
            AlbumCover.Dispose();
        }
    }
}
