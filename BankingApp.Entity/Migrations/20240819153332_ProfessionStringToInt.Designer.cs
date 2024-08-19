﻿// <auto-generated />
using System;
using BankingApp.Entity.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BankingApp.Entity.Migrations
{
    [DbContext(typeof(BankingDbContext))]
    [Migration("20240819153332_ProfessionStringToInt")]
    partial class ProfessionStringToInt
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BankingApp.Entity.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AccountNo")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Branch")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RecordScreen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("BankingApp.Entity.Entities.Credit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Approved")
                        .HasColumnType("bit");

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("FirstPaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Maturity")
                        .HasColumnType("int");

                    b.Property<float>("MonthlyInterest")
                        .HasColumnType("real");

                    b.Property<bool>("Paid")
                        .HasColumnType("bit");

                    b.Property<decimal>("PayBack")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Principal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RecordScreen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Credit");
                });

            modelBuilder.Entity("BankingApp.Entity.Entities.CreditCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<short>("BillingDay")
                        .HasColumnType("smallint");

                    b.Property<short>("CVV")
                        .HasColumnType("smallint");

                    b.Property<string>("CardNo")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<decimal>("CurrentDebt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Limit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("OutstandingBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RecordScreen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("CreditCard");
                });

            modelBuilder.Entity("BankingApp.Entity.Entities.Customer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("CreditScore")
                        .HasColumnType("int");

                    b.Property<bool>("Gender")
                        .HasColumnType("bit");

                    b.Property<string>("IdentityNo")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Profession")
                        .HasColumnType("int");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RecordScreen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("BankingApp.Entity.Entities.Login", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("IdentityNo")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<int>("Password")
                        .HasColumnType("int");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RecordScreen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Temporary")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Login");
                });

            modelBuilder.Entity("BankingApp.Entity.Entities.MailAddresses", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<string>("MailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Primary")
                        .HasColumnType("bit");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RecordScreen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("MailAddress");
                });

            modelBuilder.Entity("BankingApp.Entity.Entities.Parameter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Code")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Detail1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Detail2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Detail3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GroupCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RecordScreen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Parameter");
                });

            modelBuilder.Entity("BankingApp.Entity.Entities.TransactionHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AccountId")
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("CreditCardId")
                        .HasColumnType("int");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RecordScreen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("CreditCardId");

                    b.ToTable("TransactionHistory");
                });

            modelBuilder.Entity("BankingApp.Entity.Entities.Account", b =>
                {
                    b.HasOne("BankingApp.Entity.Entities.Customer", "Customer")
                        .WithMany("Accounts")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("BankingApp.Entity.Entities.Credit", b =>
                {
                    b.HasOne("BankingApp.Entity.Entities.Customer", "Customer")
                        .WithMany("Credits")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("BankingApp.Entity.Entities.CreditCard", b =>
                {
                    b.HasOne("BankingApp.Entity.Entities.Customer", "Customer")
                        .WithMany("CreditCards")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("BankingApp.Entity.Entities.MailAddresses", b =>
                {
                    b.HasOne("BankingApp.Entity.Entities.Customer", "Customer")
                        .WithMany("MailAddresses")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("BankingApp.Entity.Entities.TransactionHistory", b =>
                {
                    b.HasOne("BankingApp.Entity.Entities.Account", "Account")
                        .WithMany("TransactionHistory")
                        .HasForeignKey("AccountId");

                    b.HasOne("BankingApp.Entity.Entities.CreditCard", "CreditCard")
                        .WithMany()
                        .HasForeignKey("CreditCardId");

                    b.Navigation("Account");

                    b.Navigation("CreditCard");
                });

            modelBuilder.Entity("BankingApp.Entity.Entities.Account", b =>
                {
                    b.Navigation("TransactionHistory");
                });

            modelBuilder.Entity("BankingApp.Entity.Entities.Customer", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("CreditCards");

                    b.Navigation("Credits");

                    b.Navigation("MailAddresses");
                });
#pragma warning restore 612, 618
        }
    }
}
