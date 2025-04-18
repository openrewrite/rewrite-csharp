﻿using Rewrite.Test.CSharp;
using Rewrite.RewriteCSharp.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class IndexerTests : RewriteTest
{
    [Test]
    public void BasicIndexer()
    {
        RewriteRun(
            CSharp("""
                   class A
                   {
                       public int this[int index]
                       {
                           get { return 0; }
                           set { }
                       }
                   }
                   """));
    }

    [Test]
    public void IndexerWithExplicitInterface()
    {
        RewriteRun(
            CSharp("""
                   class A
                   {
                       public int IFace.this[int index]
                       {
                           get { return 0; }
                           set { }
                       }
                   }
                   """));
    }

    [Test]
    public void IndexerWithMultipleParameters()
    {
        RewriteRun(
            CSharp("""
                   class M
                   {
                       abstract int this[int r, int c]
                       {
                           get;
                           set;
                       }
                   }
                   """));
    }

    [Test]
    public void ReadOnlyIndexer()
    {
        RewriteRun(
            CSharp("""
                   class ReadOnlyList
                   {
                       public string this[int index] => items[index];
                   }
                   """));
    }

    [Test]
    public void IndexerWithAttributes()
    {
        RewriteRun(
            CSharp("""
                   class C
                   {
                       [Obsolete]
                       public object this[string name]
                       {
                           get { return null; }
                       }
                   }
                   """));
    }

    [Test]
    public void InterfaceIndexer()
    {
        RewriteRun(
            CSharp("""
                   interface IContainer
                   {
                       string this[int index] { get; set; }
                   }
                   """));
    }

    [Test]
    public void IndexerWithModifiers()
    {
        RewriteRun(
            CSharp("""
                   class Collection
                   {
                       protected internal virtual string this[int i]
                       {
                           get { return null; }
                           private set { }
                       }
                   }
                   """));
    }

    [Test]
    public void AutoImplementedIndexer()
    {
        RewriteRun(
            CSharp("""
                   class SimpleList
                   {
                       public double this[int i] { get; set; }
                   }
                   """));
    }

    [Test]
    public void ExpressionBodiedIndexer()
    {
        RewriteRun(
            CSharp("""
                   class Container
                   {
                       private string[] items;
                       public string this[int i]
                       {
                           get => items[i];
                           set => items[i] = value;
                       }
                   }
                   """));
    }
}


