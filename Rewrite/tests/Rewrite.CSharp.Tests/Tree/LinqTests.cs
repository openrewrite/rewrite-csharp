using FluentAssertions;
using Rewrite.Test.CSharp;
using Rewrite.RewriteJava.Tree;
using Rewrite.Test;

namespace Rewrite.CSharp.Tests.Tree;

using static Assertions;

public class LinqTests : RewriteTest
{
    [Test]
    public void SimpleSelectClause()
    {
        RewriteRun(
            CSharp(
                """
                var query = from x in numbers
                           select x;
                """
            )
        );
    }

    [Test]
    public void SelectWithProjection()
    {
        RewriteRun(
            CSharp(
                """
                var query = from person in people
                           select new { Name = person.Name, Age = person.Age };
                """
            )
        );
    }

    [Test]
    public void SimpleGroupBy()
    {
        RewriteRun(
            CSharp(
                """
                var query = from p in people
                           group p by p.State;
                """
            )
        );
    }

    [Test]
    public void GroupByWithProjection()
    {
        RewriteRun(
            CSharp(
                """
                var query = from p in people
                           group p by new { p.State, p.City };
                """
            )
        );
    }

    [Test]
    public void SimpleOrderBy()
    {
        RewriteRun(
            CSharp(
                """
                var query = from p in people
                           orderby p.Age
                           select p;
                """
            )
        );
    }

    [Test]
    public void OrderByWithDirection()
    {
        RewriteRun(
            CSharp(
                """
                var query = from p in people
                           orderby p.Age descending
                           select p;
                """
            )
        );
    }

    [Test]
    public void MultipleOrderings()
    {
        RewriteRun(
            CSharp(
                """
                var query = from p in people
                           orderby p.LastName ascending, p.FirstName ascending, p.Age descending
                           select p;
                """
            )
        );
    }

    [Test]
    public void JoinInto()
    {
        RewriteRun(
            CSharp(
                """
                from ou in orgUsers
                join o in orgs on ou.orgId equals o.Id into outerOrg
                select o;
                """
            )
        );
    }

    [Test]
    public void ComplexLinqQuery()
    {
        RewriteRun(
            CSharp(
                """
                var query = from p in people
                           where p.Age > 21
                           group p by p.State into g
                           orderby g.Key
                           select new {
                               State = g.Key,
                               Count = g.Count()
                           };
                """
            )
        );
    }
}
