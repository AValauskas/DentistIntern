using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DentistIntern.Models
{
    public partial class dentistdbContext : DbContext
    {
        public dentistdbContext()
        {
        }

        public dentistdbContext(DbContextOptions<dentistdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<FreeTime> FreeTime { get; set; }
        public virtual DbSet<Lecturer> Lecturer { get; set; }
        public virtual DbSet<Response> Response { get; set; }
        public virtual DbSet<Session> Session { get; set; }
        public virtual DbSet<Trainee> Trainee { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:monopol.database.windows.net,1433;Initial Catalog=dentistdb;Persist Security Info=False;User ID=aurval10;Password=aur.val10;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.IdClient);

                entity.Property(e => e.IdClient).HasColumnName("id_Client");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phone_number")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .HasColumnName("surname")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FreeTime>(entity =>
            {
                entity.HasKey(e => e.IdFreeTime);

                entity.Property(e => e.IdFreeTime).HasColumnName("id_FreeTime");

                entity.Property(e => e.FkTraineeidTrainee).HasColumnName("fk_Traineeid_Trainee");

                entity.Property(e => e.TimeEnd)
                    .HasColumnName("time_end")
                    .HasColumnType("datetime");

                entity.Property(e => e.TimeStart)
                    .HasColumnName("time_start")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.FkTraineeidTraineeNavigation)
                    .WithMany(p => p.FreeTime)
                    .HasForeignKey(d => d.FkTraineeidTrainee)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__FreeTime__fk_Tra__5070F446");
            });

            modelBuilder.Entity<Lecturer>(entity =>
            {
                entity.HasKey(e => e.IdLecturer);

                entity.Property(e => e.IdLecturer).HasColumnName("id_Lecturer");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phone_number")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .HasColumnName("surname")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Response>(entity =>
            {
                entity.HasKey(e => e.IdResponse);

                entity.Property(e => e.IdResponse).HasColumnName("id_Response");

                entity.Property(e => e.FkClientidClient).HasColumnName("fk_Clientid_Client");

                entity.Property(e => e.FkTraineeidTrainee).HasColumnName("fk_Traineeid_Trainee");

                entity.Property(e => e.Mark).HasColumnName("mark");

                entity.Property(e => e.Text)
                    .HasColumnName("text")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.FkClientidClientNavigation)
                    .WithMany(p => p.Response)
                    .HasForeignKey(d => d.FkClientidClient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Response__fk_Cli__5441852A");

                entity.HasOne(d => d.FkTraineeidTraineeNavigation)
                    .WithMany(p => p.Response)
                    .HasForeignKey(d => d.FkTraineeidTrainee)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Response__fk_Tra__534D60F1");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasKey(e => e.IdSession);

                entity.Property(e => e.IdSession).HasColumnName("id_Session");

                entity.Property(e => e.FkClientidClient).HasColumnName("fk_Clientid_Client");

                entity.Property(e => e.FkFreeTimeidFreeTime).HasColumnName("fk_FreeTimeid_FreeTime");

                entity.Property(e => e.TimeEnd)
                    .HasColumnName("time_end")
                    .HasColumnType("datetime");

                entity.Property(e => e.TimeStart)
                    .HasColumnName("time_start")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.FkClientidClientNavigation)
                    .WithMany(p => p.Session)
                    .HasForeignKey(d => d.FkClientidClient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Session__fk_Clie__571DF1D5");

                entity.HasOne(d => d.FkFreeTimeidFreeTimeNavigation)
                    .WithMany(p => p.Session)
                    .HasForeignKey(d => d.FkFreeTimeidFreeTime)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Session__fk_Free__5812160E");
            });

            modelBuilder.Entity<Trainee>(entity =>
            {
                entity.HasKey(e => e.IdTrainee);

                entity.Property(e => e.IdTrainee).HasColumnName("id_Trainee");

                entity.Property(e => e.Experience).HasColumnName("experience");

                entity.Property(e => e.FkLectureridLecturer).HasColumnName("fk_Lecturerid_Lecturer");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phone_number")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .HasColumnName("surname")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.FkLectureridLecturerNavigation)
                    .WithMany(p => p.Trainee)
                    .HasForeignKey(d => d.FkLectureridLecturer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Trainee__fk_Lect__4D94879B");
            });
        }
    }
}
