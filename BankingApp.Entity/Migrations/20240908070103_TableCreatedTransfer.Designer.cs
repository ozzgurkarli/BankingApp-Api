﻿// <auto-generated />
using System;
using BankingApp.Entity.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BankingApp.Entity.Migrations
{
    [DbContext(typeof(BankingDbContext))]
    [Migration("20240908070103_TableCreatedTransfer")]
    partial class TableCreatedTransfer
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BankingApp.Entity.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AccountNo")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric");

                    b.Property<int>("Branch")
                        .HasColumnType("integer");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<bool>("Primary")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RecordScreen")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("BankingApp.Entity.Entities.AccountTracker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Currency")
                        .HasColumnType("text");

                    b.Property<string>("FirstAvailableNo")
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RecordScreen")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AccountTracker");
                });

            modelBuilder.Entity("BankingApp.Entity.Entities.Credit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Approved")
                        .HasColumnType("boolean");

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("FirstPaymentDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Maturity")
                        .HasColumnType("integer");

                    b.Property<float>("MonthlyInterest")
                        .HasColumnType("real");

                    b.Property<bool>("Paid")
                        .HasColumnType("boolean");

                    b.Property<decimal>("PayBack")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Principal")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RecordScreen")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Credit");
                });

            modelBuilder.Entity("BankingApp.Entity.Entities.CreditCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<short>("BillingDay")
                        .HasColumnType("smallint");

                    b.Property<short>("CVV")
                        .HasColumnType("smallint");

                    b.Property<string>("CardNo")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.Property<decimal>("CurrentDebt")
                        .HasColumnType("numeric");

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("Limit")
                        .HasColumnType("numeric");

                    b.Property<decimal>("OutstandingBalance")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RecordScreen")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("TotalDebt")
                        .HasColumnType("numeric");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("CreditCard");
                });

            modelBuilder.Entity("BankingApp.Entity.Entities.Customer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 100000000000L)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<int>("CreditScore")
                        .HasColumnType("integer");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("IdentityNo")
                        .HasMaxLength(11)
                        .HasColumnType("character varying(11)");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNo")
                        .HasColumnType("text");

                    b.Property<int>("Profession")
                        .HasColumnType("integer");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RecordScreen")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Salary")
                        .HasColumnType("numeric");

                    b.Property<string>("Surname")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("BankingApp.Entity.Entities.Login", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("IdentityNo")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("character varying(11)");

                    b.Property<int>("Password")
                        .HasColumnType("integer");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RecordScreen")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Temporary")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Login");
                });

            modelBuilder.Entity("BankingApp.Entity.Entities.MailAddresses", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<long>("CustomerId")
                        .HasColumnType("bigint");

                    b.Property<string>("MailAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Primary")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RecordScreen")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("MailAddress");
                });

            modelBuilder.Entity("BankingApp.Entity.Entities.Parameter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Code")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Detail1")
                        .HasColumnType("text");

                    b.Property<string>("Detail2")
                        .HasColumnType("text");

                    b.Property<string>("Detail3")
                        .HasColumnType("text");

                    b.Property<string>("GroupCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RecordScreen")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Parameter");
                });

            modelBuilder.Entity("BankingApp.Entity.Entities.TransactionHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("AccountId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<int?>("CreditCardId")
                        .HasColumnType("integer");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime>("RecordDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RecordScreen")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("timestamp with time zone");

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
