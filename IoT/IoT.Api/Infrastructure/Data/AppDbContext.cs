using System;
using System.Collections.Generic;
using IoT.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IoT.Api.Infrastructure.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }


    public virtual DbSet<DEVICE_METRIC> DEVICE_METRICs { get; set; }

    public virtual DbSet<IOT_ALERT> IOT_ALERTs { get; set; }

    public virtual DbSet<IOT_CONNECTION_LOG> IOT_CONNECTION_LOGs { get; set; }

    public virtual DbSet<IOT_DEVICE> IOT_DEVICEs { get; set; }

    public virtual DbSet<IOT_DEVICE_CONFIG> IOT_DEVICE_CONFIGs { get; set; }

    public virtual DbSet<IOT_DEVICE_DATum> IOT_DEVICE_DATAs { get; set; }

    public virtual DbSet<MACHINE> MACHINEs { get; set; }

    public virtual DbSet<MACHINE_STATUS_LOG> MACHINE_STATUS_LOGs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<DEVICE_METRIC>(entity =>
        {
            entity.HasKey(e => e.METRIC_ID).HasName("SYS_C008335");

            entity.ToTable("DEVICE_METRICS");

            entity.HasIndex(e => e.METRIC_CODE, "SYS_C008336").IsUnique();

            entity.Property(e => e.METRIC_ID)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER");
            entity.Property(e => e.CREATED_AT)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP\n");
            entity.Property(e => e.MAX_VALUE).HasColumnType("NUMBER");
            entity.Property(e => e.METRIC_CODE)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.METRIC_NAME)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MIN_VALUE).HasColumnType("NUMBER");
            entity.Property(e => e.UNIT)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<IOT_ALERT>(entity =>
        {
            entity.HasKey(e => e.ALERT_ID).HasName("SYS_C008351");

            entity.ToTable("IOT_ALERTS");

            entity.HasIndex(e => e.CREATED_AT, "IDX_ALERT_TIME");

            entity.Property(e => e.ALERT_ID)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER");
            entity.Property(e => e.ALERT_TYPE)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CREATED_AT)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.DEVICE_ID).HasColumnType("NUMBER");
            entity.Property(e => e.MESSAGE)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.METRIC_CODE)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.DEVICE).WithMany(p => p.IOT_ALERTs)
                .HasForeignKey(d => d.DEVICE_ID)
                .HasConstraintName("FK_ALERT_DEVICE");
        });

        modelBuilder.Entity<IOT_CONNECTION_LOG>(entity =>
        {
            entity.HasKey(e => e.LOG_ID).HasName("SYS_C008344");

            entity.ToTable("IOT_CONNECTION_LOGS");

            entity.Property(e => e.LOG_ID)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER");
            entity.Property(e => e.CREATED_AT)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.DEVICE_ID).HasColumnType("NUMBER");
            entity.Property(e => e.MESSAGE)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.STATUS)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.DEVICE).WithMany(p => p.IOT_CONNECTION_LOGs)
                .HasForeignKey(d => d.DEVICE_ID)
                .HasConstraintName("FK_CONN_DEVICE");
        });

        modelBuilder.Entity<IOT_DEVICE>(entity =>
        {
            entity.HasKey(e => e.DEVICE_ID).HasName("SYS_C008326");

            entity.ToTable("IOT_DEVICES");

            entity.HasIndex(e => e.DEVICE_CODE, "SYS_C008327").IsUnique();

            entity.Property(e => e.DEVICE_ID)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER");
            entity.Property(e => e.CREATED_AT)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.DEVICE_CODE)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IP_ADDRESS)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LAST_CONNECTED).HasPrecision(6);
            entity.Property(e => e.MACHINE_ID).HasColumnType("NUMBER");
            entity.Property(e => e.PORT).HasColumnType("NUMBER");
            entity.Property(e => e.PROTOCOL)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.STATUS)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.MACHINE).WithMany(p => p.IOT_DEVICEs)
                .HasForeignKey(d => d.MACHINE_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DEVICE_MACHINE");
        });

        modelBuilder.Entity<IOT_DEVICE_CONFIG>(entity =>
        {
            entity.HasKey(e => e.CONFIG_ID).HasName("SYS_C008331");

            entity.ToTable("IOT_DEVICE_CONFIG");

            entity.Property(e => e.CONFIG_ID)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER");
            entity.Property(e => e.DEVICE_ID).HasColumnType("NUMBER");
            entity.Property(e => e.PARAM_NAME)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PARAM_VALUE)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UPDATED_AT)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.DEVICE).WithMany(p => p.IOT_DEVICE_CONFIGs)
                .HasForeignKey(d => d.DEVICE_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CONFIG_DEVICE");
        });

        modelBuilder.Entity<IOT_DEVICE_DATum>(entity =>
        {
            entity.HasKey(e => e.DATA_ID).HasName("SYS_C008340");

            entity.ToTable("IOT_DEVICE_DATA");

            entity.HasIndex(e => e.DEVICE_ID, "IDX_IOT_DATA_DEVICE");

            entity.HasIndex(e => e.RECORDED_AT, "IDX_IOT_DATA_TIME");

            entity.Property(e => e.DATA_ID)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER");
            entity.Property(e => e.DATA_JSON).HasColumnType("CLOB");
            entity.Property(e => e.DEVICE_ID).HasColumnType("NUMBER");
            entity.Property(e => e.RECORDED_AT)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.DEVICE).WithMany(p => p.IOT_DEVICE_DATa)
                .HasForeignKey(d => d.DEVICE_ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DATA_DEVICE");
        });

        modelBuilder.Entity<MACHINE>(entity =>
        {
            entity.HasKey(e => e.MACHINE_ID).HasName("SYS_C008319");

            entity.ToTable("MACHINES");

            entity.HasIndex(e => e.MACHINE_CODE, "SYS_C008320").IsUnique();

            entity.Property(e => e.MACHINE_ID)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER");
            entity.Property(e => e.CREATED_AT)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP\n");
            entity.Property(e => e.LOCATION)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MACHINE_CODE)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MACHINE_NAME)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MACHINE_TYPE)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.STATUS)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MACHINE_STATUS_LOG>(entity =>
        {
            entity.HasKey(e => e.LOG_ID).HasName("SYS_C008348");

            entity.ToTable("MACHINE_STATUS_LOGS");

            entity.Property(e => e.LOG_ID)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER");
            entity.Property(e => e.CREATED_AT)
                .HasPrecision(6)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.MACHINE_ID).HasColumnType("NUMBER");
            entity.Property(e => e.MESSAGE)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.STATUS)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.MACHINE).WithMany(p => p.MACHINE_STATUS_LOGs)
                .HasForeignKey(d => d.MACHINE_ID)
                .HasConstraintName("FK_MACHINE_LOG");
        });
        modelBuilder.HasSequence("LOGMNR_DIDS$");
        modelBuilder.HasSequence("LOGMNR_EVOLVE_SEQ$");
        modelBuilder.HasSequence("LOGMNR_SEQ$");
        modelBuilder.HasSequence("LOGMNR_UIDS$").IsCyclic();
        modelBuilder.HasSequence("MVIEW$_ADVSEQ_GENERIC");
        modelBuilder.HasSequence("MVIEW$_ADVSEQ_ID");
        modelBuilder.HasSequence("ROLLING_EVENT_SEQ$");
        modelBuilder.HasSequence("SEQ_BOOKINGS");
        modelBuilder.HasSequence("SEQ_MOVIES");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
