using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntityConfigs
{
    public class WorldConfiguration : IEntityTypeConfiguration<World>
    {
        public void Configure(EntityTypeBuilder<World> builder)
        {
            builder.HasData
            (
                new World
                {
                    Id = new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"),
                    Title = "استارت آپ و کسب و کار"
                },
                new World
                {
                    Id = new Guid("f68eb329-64f6-425c-9052-f0cf42117524"),
                    Title = "آموزشی"
                },
                new World
                {
                    Id = new Guid("6018a1df-ad09-4005-8c36-29db689fc7e6"),
                    Title = "اقتصاد، بورس، مالی، سرمایه گذاری"
                },
                new World
                {
                    Id = new Guid("e8bdbbdf-4624-4501-9c39-505c60316c1b"),
                    Title = "حقوقی، قضایی، وکالت"
                },
                new World
                {
                    Id = new Guid("17d0f3ef-6ff3-4868-b924-c13c5c45ab49"),
                    Title = "ورزشی، مد، زیبایی و تغذیه"
                },
                new World
                {
                    Id = new Guid("0f3ac0a6-655e-4498-8590-eaabd5788e9a"),
                    Title = "مشاوره و روانشناسی"
                },
                new World
                {
                    Id = new Guid("2cb6f432-2472-4681-8f22-4c4a94853f0d"),
                    Title = "بیمه، مالیات، شرکت"
                },
                new World
                {
                    Id = new Guid("7638e5c6-cba1-4071-9a0b-ecff4d9c645d"),
                    Title = "سلامتی و پزشکی"
                },
                new World
                {
                    Id = new Guid("c0f3220a-d688-4535-ae72-feabe2e61f2c"),
                    Title = "سرمایه انسانی و توسعه فردی"
                },
                new World
                {
                    Id = new Guid("c5df6845-f8c0-4062-9ebd-7079a5a5f615"),
                    Title = "مهاجرت و ادامه ی تحصیل"
                },
                new World
                {
                    Id = new Guid("8f00ea20-aec9-4051-ba2c-9397ee406e63"),
                    Title = "نظلم وظیفه و اداری"
                },
                new World
                {
                    Id = new Guid("e1c1ed84-b379-4c24-9121-c1c6263491b3"),
                    Title = "فلسفه، دین، مذهب"
                },
                new World
                {
                    Id = new Guid("ee19c0cc-ed03-4c6b-84cd-a1e5f31ecada"),
                    Title = "خرید و فروش"
                },
                new World
                {
                    Id = new Guid("07b2f146-b624-4c8e-bdf0-e5172c5e9f1c"),
                    Title = "فرهنگ، هنر، معماری"
                },
                new World
                {
                    Id = new Guid("edf6a3f8-c2f1-454d-85e9-11eab8fad28f"),
                    Title = "تکنولوژی و مهندسی"
                },
                new World
                {
                    Id = new Guid("4bf0d2a9-b62b-4f58-a18b-a1eef5c3b9c7"),
                    Title = "تفریح، سرگرمی، سفر"
                },
                new World
                {
                    Id = new Guid("10fd49fd-2187-406a-830c-b367035cd448"),
                    Title = "سیاست"
                }
            );
        }
    }
}