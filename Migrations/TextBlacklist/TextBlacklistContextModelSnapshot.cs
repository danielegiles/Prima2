﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Prima.Contexts;

namespace Prima.Migrations.TextBlacklist
{
    [DbContext(typeof(TextBlacklistContext))]
    partial class TextBlacklistContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1");

            modelBuilder.Entity("Prima.Contexts.GuildTextBlacklistEntry", b =>
                {
                    b.Property<string>("RegexString")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("GuildId")
                        .HasColumnType("INTEGER");

                    b.HasKey("RegexString");

                    b.ToTable("RegexStrings");
                });
#pragma warning restore 612, 618
        }
    }
}
