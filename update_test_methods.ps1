$testFiles = Get-ChildItem -Path "Rewrite\tests\Rewrite.CSharp.Tests" -Filter "*.cs" -Recurse | 
    Where-Object { 
        $_.FullName -notlike "*\obj\*" -and 
        $_.FullName -notlike "*\bin\*" 
    }

$totalFiles = $testFiles.Count
$modifiedFiles = 0

foreach ($file in $testFiles) {
    $content = Get-Content $file.FullName -Raw
    $originalContent = $content
    
    # Pattern to match test methods with various test attributes
    # Match patterns like:
    # [Test] private void MethodName
    # [TestCase...] private void MethodName
    # [Theory] private void MethodName
    # [Fact] private void MethodName
    
    # Replace private test methods with public
    $patterns = @(
        # Pattern for [Test] attribute followed by private method
        '(\[Test\])\s*\n\s*private\s+((?:async\s+)?(?:static\s+)?(?:virtual\s+)?(?:override\s+)?(?:sealed\s+)?(?:Task<.*?>|Task|void|[\w<>,\[\]]+))\s+(\w+)',
        # Pattern for [TestCase] attribute followed by private method
        '(\[TestCase[^\]]*\])\s*\n\s*private\s+((?:async\s+)?(?:static\s+)?(?:virtual\s+)?(?:override\s+)?(?:sealed\s+)?(?:Task<.*?>|Task|void|[\w<>,\[\]]+))\s+(\w+)',
        # Pattern for [Theory] attribute followed by private method
        '(\[Theory[^\]]*\])\s*\n\s*private\s+((?:async\s+)?(?:static\s+)?(?:virtual\s+)?(?:override\s+)?(?:sealed\s+)?(?:Task<.*?>|Task|void|[\w<>,\[\]]+))\s+(\w+)',
        # Pattern for [Fact] attribute followed by private method
        '(\[Fact[^\]]*\])\s*\n\s*private\s+((?:async\s+)?(?:static\s+)?(?:virtual\s+)?(?:override\s+)?(?:sealed\s+)?(?:Task<.*?>|Task|void|[\w<>,\[\]]+))\s+(\w+)'
    )
    
    foreach ($pattern in $patterns) {
        $content = $content -replace $pattern, '$1`n    public $2 $3'
    }
    
    # Also handle cases where methods don't have explicit access modifiers (default private in C#)
    # But only for methods with test attributes
    $implicitPrivatePatterns = @(
        '(\[Test\])\s*\n\s+(?!public|private|protected|internal)((?:async\s+)?(?:static\s+)?(?:virtual\s+)?(?:override\s+)?(?:sealed\s+)?(?:Task<.*?>|Task|void|[\w<>,\[\]]+))\s+(\w+)',
        '(\[TestCase[^\]]*\])\s*\n\s+(?!public|private|protected|internal)((?:async\s+)?(?:static\s+)?(?:virtual\s+)?(?:override\s+)?(?:sealed\s+)?(?:Task<.*?>|Task|void|[\w<>,\[\]]+))\s+(\w+)',
        '(\[Theory[^\]]*\])\s*\n\s+(?!public|private|protected|internal)((?:async\s+)?(?:static\s+)?(?:virtual\s+)?(?:override\s+)?(?:sealed\s+)?(?:Task<.*?>|Task|void|[\w<>,\[\]]+))\s+(\w+)',
        '(\[Fact[^\]]*\])\s*\n\s+(?!public|private|protected|internal)((?:async\s+)?(?:static\s+)?(?:virtual\s+)?(?:override\s+)?(?:sealed\s+)?(?:Task<.*?>|Task|void|[\w<>,\[\]]+))\s+(\w+)'
    )
    
    foreach ($pattern in $implicitPrivatePatterns) {
        $content = $content -replace $pattern, '$1`n    public $2 $3'
    }
    
    # Only write back if content has changed
    if ($content -ne $originalContent) {
        Set-Content -Path $file.FullName -Value $content -NoNewline
        $modifiedFiles++
        Write-Host "Modified: $($file.Name)"
    }
}

Write-Host "`nTotal files processed: $totalFiles"
Write-Host "Files modified: $modifiedFiles"