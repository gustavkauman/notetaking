﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Notetaking;

#nullable disable

namespace Notetaking.Migrations
{
    [DbContext(typeof(NotetakingContext))]
    partial class NotetakingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.4");

            modelBuilder.Entity("Notetaking.Models.NoteRelation", b =>
                {
                    b.Property<int>("FromNoteId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ToNoteId")
                        .HasColumnType("INTEGER");

                    b.HasKey("FromNoteId", "ToNoteId");

                    b.HasIndex("ToNoteId");

                    b.ToTable("NoteRelation");
                });

            modelBuilder.Entity("Notetaking.Note", b =>
                {
                    b.Property<int>("NoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Body")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("NoteId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("Notetaking.Models.NoteRelation", b =>
                {
                    b.HasOne("Notetaking.Note", "FromNote")
                        .WithMany("FromNoteRelations")
                        .HasForeignKey("FromNoteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Notetaking.Note", "ToNote")
                        .WithMany("ToNoteRelations")
                        .HasForeignKey("ToNoteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FromNote");

                    b.Navigation("ToNote");
                });

            modelBuilder.Entity("Notetaking.Note", b =>
                {
                    b.Navigation("FromNoteRelations");

                    b.Navigation("ToNoteRelations");
                });
#pragma warning restore 612, 618
        }
    }
}
