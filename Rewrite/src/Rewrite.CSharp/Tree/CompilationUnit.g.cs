//------------------------------------------------------------------------------
// <auto-generated>
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
#pragma warning disable CS0108
using System.Diagnostics.CodeAnalysis;
using Rewrite.Core;
using Rewrite.Core.Marker;
using FileAttributes = Rewrite.Core.FileAttributes;
using Rewrite.RewriteJava.Tree;

namespace Rewrite.RewriteCSharp.Tree;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PossibleUnintendedReferenceComparison")]
[SuppressMessage("ReSharper", "InvertIf")]
[SuppressMessage("ReSharper", "RedundantExtendsListEntry")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "RedundantNameQualifier")]
public partial interface Cs : J
{
    public partial class CompilationUnit(
    Guid id,
    Space prefix,
    Markers markers,
    string sourcePath,
    FileAttributes? fileAttributes,
    string? charsetName,
    bool charsetBomMarked,
    Checksum? checksum,
    IList<JRightPadded<ExternAlias>> externs,
    IList<JRightPadded<UsingDirective>> usings,
    IList<AttributeList> attributeLists,
    IList<JRightPadded<Statement>> members,
    Space eof
    ) : Cs, JavaSourceFile<CompilationUnit>, MutableTree<CompilationUnit>
    {
        [NonSerialized] private WeakReference<PaddingHelper>? _padding;

        public PaddingHelper Padding
        {
            get
            {
                PaddingHelper? p;
                if (_padding == null)
                {
                    p = new PaddingHelper(this);
                    _padding = new WeakReference<PaddingHelper>(p);
                }
                else
                {
                    _padding.TryGetTarget(out p);
                    if (p == null || p.T != this)
                    {
                        p = new PaddingHelper(this);
                        _padding.SetTarget(p);
                    }
                }
                return p;
            }
        }

        public J? AcceptCSharp<P>(CSharpVisitor<P> v, P p)
        {
            return v.VisitCompilationUnit(this, p);
        }

        public Guid Id => id;

        public CompilationUnit WithId(Guid newId)
        {
            return newId == id ? this : new CompilationUnit(newId, prefix, markers, sourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, _externs, _usings, attributeLists, _members, eof);
        }
        public Space Prefix => prefix;

        public CompilationUnit WithPrefix(Space newPrefix)
        {
            return newPrefix == prefix ? this : new CompilationUnit(id, newPrefix, markers, sourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, _externs, _usings, attributeLists, _members, eof);
        }
        public Markers Markers => markers;

        public CompilationUnit WithMarkers(Markers newMarkers)
        {
            return ReferenceEquals(newMarkers, markers) ? this : new CompilationUnit(id, prefix, newMarkers, sourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, _externs, _usings, attributeLists, _members, eof);
        }
        public string SourcePath => sourcePath;

        public CompilationUnit WithSourcePath(string newSourcePath)
        {
            return newSourcePath == sourcePath ? this : new CompilationUnit(id, prefix, markers, newSourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, _externs, _usings, attributeLists, _members, eof);
        }
        public FileAttributes? FileAttributes => fileAttributes;

        public CompilationUnit WithFileAttributes(FileAttributes? newFileAttributes)
        {
            return newFileAttributes == fileAttributes ? this : new CompilationUnit(id, prefix, markers, sourcePath, newFileAttributes, charsetName, charsetBomMarked, checksum, _externs, _usings, attributeLists, _members, eof);
        }
        public string? CharsetName => charsetName;

        public CompilationUnit WithCharsetName(string? newCharsetName)
        {
            return newCharsetName == charsetName ? this : new CompilationUnit(id, prefix, markers, sourcePath, fileAttributes, newCharsetName, charsetBomMarked, checksum, _externs, _usings, attributeLists, _members, eof);
        }
        public bool CharsetBomMarked => charsetBomMarked;

        public CompilationUnit WithCharsetBomMarked(bool newCharsetBomMarked)
        {
            return newCharsetBomMarked == charsetBomMarked ? this : new CompilationUnit(id, prefix, markers, sourcePath, fileAttributes, charsetName, newCharsetBomMarked, checksum, _externs, _usings, attributeLists, _members, eof);
        }
        public Checksum? Checksum => checksum;

        public CompilationUnit WithChecksum(Checksum? newChecksum)
        {
            return newChecksum == checksum ? this : new CompilationUnit(id, prefix, markers, sourcePath, fileAttributes, charsetName, charsetBomMarked, newChecksum, _externs, _usings, attributeLists, _members, eof);
        }
        private readonly IList<JRightPadded<Cs.ExternAlias>> _externs = externs;
        public IList<Cs.ExternAlias> Externs => _externs.Elements();

        public CompilationUnit WithExterns(IList<Cs.ExternAlias> newExterns)
        {
            return Padding.WithExterns(_externs.WithElements(newExterns));
        }
        private readonly IList<JRightPadded<Cs.UsingDirective>> _usings = usings;
        public IList<Cs.UsingDirective> Usings => _usings.Elements();

        public CompilationUnit WithUsings(IList<Cs.UsingDirective> newUsings)
        {
            return Padding.WithUsings(_usings.WithElements(newUsings));
        }
        public IList<Cs.AttributeList> AttributeLists => attributeLists;

        public CompilationUnit WithAttributeLists(IList<Cs.AttributeList> newAttributeLists)
        {
            return newAttributeLists == attributeLists ? this : new CompilationUnit(id, prefix, markers, sourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, _externs, _usings, newAttributeLists, _members, eof);
        }
        private readonly IList<JRightPadded<Statement>> _members = members;
        public IList<Statement> Members => _members.Elements();

        public CompilationUnit WithMembers(IList<Statement> newMembers)
        {
            return Padding.WithMembers(_members.WithElements(newMembers));
        }
        public Space Eof => eof;

        public CompilationUnit WithEof(Space newEof)
        {
            return newEof == eof ? this : new CompilationUnit(id, prefix, markers, sourcePath, fileAttributes, charsetName, charsetBomMarked, checksum, _externs, _usings, attributeLists, _members, newEof);
        }
        public sealed record PaddingHelper(Cs.CompilationUnit T)
        {
            public IList<JRightPadded<Cs.ExternAlias>> Externs => T._externs;

            public Cs.CompilationUnit WithExterns(IList<JRightPadded<Cs.ExternAlias>> newExterns)
            {
                return T._externs == newExterns ? T : new Cs.CompilationUnit(T.Id, T.Prefix, T.Markers, T.SourcePath, T.FileAttributes, T.CharsetName, T.CharsetBomMarked, T.Checksum, newExterns, T._usings, T.AttributeLists, T._members, T.Eof);
            }

            public IList<JRightPadded<Cs.UsingDirective>> Usings => T._usings;

            public Cs.CompilationUnit WithUsings(IList<JRightPadded<Cs.UsingDirective>> newUsings)
            {
                return T._usings == newUsings ? T : new Cs.CompilationUnit(T.Id, T.Prefix, T.Markers, T.SourcePath, T.FileAttributes, T.CharsetName, T.CharsetBomMarked, T.Checksum, T._externs, newUsings, T.AttributeLists, T._members, T.Eof);
            }

            public IList<JRightPadded<Statement>> Members => T._members;

            public Cs.CompilationUnit WithMembers(IList<JRightPadded<Statement>> newMembers)
            {
                return T._members == newMembers ? T : new Cs.CompilationUnit(T.Id, T.Prefix, T.Markers, T.SourcePath, T.FileAttributes, T.CharsetName, T.CharsetBomMarked, T.Checksum, T._externs, T._usings, T.AttributeLists, newMembers, T.Eof);
            }

        }

        public bool Equals(Rewrite.Core.Tree? other)
        {
            return other is CompilationUnit && other.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public ITreeVisitor<Core.Tree, PrintOutputCapture<P>> Printer<P>(Cursor cursor)
        {
            return IPrinterFactory.Current()!.CreatePrinter<P>();
        }
    }
}