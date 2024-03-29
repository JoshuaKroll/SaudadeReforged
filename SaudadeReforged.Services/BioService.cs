﻿using SaudadeReforged.Data;
using SaudadeReforged.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaudadeReforged.Services
{
    public class BioService
    {
        private readonly Guid _userId;

        public Guid OwnerId { get; private set; }

        public BioService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateBio(BioCreate model)
        {
            var entity =
                new Bio()
                {
                    OwnerId = _userId,
                    FullName = model.FullName,
                    NickNames = model.NickNames,
                    Birthday = model.Birthday,
                    Age = model.Age,
                    Gender = model.Gender,
                    Location = model.Location,
                    Race = model.Race,
                    Ethnicity = model.Ethnicity,
                    AboutYou = model.AboutYou,
                    Interests = model.Interests,
                    Hobbies = model.Hobbies,

                    CreatedUtc = DateTimeOffset.Now
                };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Bios.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<BioListItem> GetBios()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Bios
                    .Where(e => e.OwnerId == _userId)
                    .Select(
                        e =>
                        new BioListItem
                        {
                            BioId = e.BioId,
                            FullName = e.FullName,
                            NickNames = e.NickNames,
                            Birthday = e.Birthday,
                            Age = e.Age,
                            Gender = e.Gender,
                            Location = e.Location,
                            Race = e.Race,
                            Ethnicity = e.Ethnicity,
                            AboutYou = e.AboutYou,
                            Interests = e.Interests,
                            Hobbies = e.Hobbies,
                            CreatedUtc = e.CreatedUtc
                        }
                   );
                return query.ToArray();
            }
        }

        public BioDetails GetBioById(int bioId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Bios
                    .Single(e => e.BioId == bioId && e.OwnerId == _userId);
                return
                    new BioDetails
                    {
                        BioId = entity.BioId,
                        FullName = entity.FullName,
                        NickNames = entity.NickNames,
                        Birthday = entity.Birthday,
                        Age = entity.Age,
                        Gender = entity.Gender,
                        Location = entity.Location,
                        Race = entity.Race,
                        Ethnicity = entity.Ethnicity,
                        AboutYou = entity.AboutYou,
                        Interests = entity.Interests,
                        Hobbies = entity.Hobbies,
                        CreatedUtc = entity.CreatedUtc,
                        ModifiedUtc = entity.ModifiedUtc
                    };
            }
        }

        public bool UpdateBio(BioEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Bios
                    .Single(e => e.BioId == model.BioId && e.OwnerId == _userId);

                entity.FullName = model.FullName;
                entity.NickNames = model.NickNames;
                entity.Birthday = model.Birthday;
                entity.Age = model.Age;
                entity.Gender = model.Gender;
                entity.Location = model.Location;
                entity.Race = model.Race;
                entity.Ethnicity = model.Ethnicity;
                entity.AboutYou = model.AboutYou;
                entity.Interests = model.Interests;
                entity.Hobbies = model.Hobbies;
                entity.ModifiedUtc = DateTimeOffset.UtcNow;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteBio(int bioId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Bios
                    .Single(e => e.BioId == bioId && e.OwnerId == _userId);

                ctx.Bios.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
