#!/usr/bin/env python3
"""
Extract type information from Cs.java file
"""
import re
import sys

def extract_type_info(file_path):
    with open(file_path, 'r', encoding='utf-8') as f:
        content = f.read()
    
    types = []
    lines = content.split('\n')
    
    i = 0
    while i < len(lines):
        line = lines[i].strip()
        
        # Look for class/interface/enum definitions
        if re.match(r'^(final|public|static)?\s*(class|interface|enum)\s+\w+', line):
            # Extract type name and what it extends/implements
            type_match = re.search(r'(class|interface|enum)\s+(\w+)(?:\s+(?:extends|implements)\s+([^{]+))?', line)
            if type_match:
                type_kind = type_match.group(1)
                type_name = type_match.group(2)
                extends_implements = type_match.group(3).strip() if type_match.group(3) else None
                
                # Skip Padding classes
                if type_name == 'Padding':
                    i += 1
                    continue
                
                print(f"\n{type_kind.upper()}: {type_name}")
                if extends_implements:
                    print(f"  Extends/Implements: {extends_implements}")
                
                # Look for fields in this type
                i += 1
                brace_count = 0
                fields = []
                
                while i < len(lines):
                    current_line = lines[i].strip()
                    
                    # Count braces to track nested scope
                    brace_count += current_line.count('{') - current_line.count('}')
                    
                    # If we're back to the original scope level, we're done with this type
                    if brace_count < 0:
                        break
                    
                    # Look for field declarations
                    # Pattern matches: [@annotations] [modifiers] Type fieldName;
                    field_match = re.match(r'^(@\w+\s+)*\s*(@?\w+[\w<>@\[\]\.\s]*)\s+(\w+)\s*;', current_line)
                    if field_match and not current_line.startswith('//'):
                        field_type = field_match.group(2).strip()
                        field_name = field_match.group(3)
                        
                        # Check if @Nullable annotation is present in previous lines
                        nullable = False
                        for j in range(max(0, i-5), i):
                            if '@Nullable' in lines[j]:
                                nullable = True
                                break
                        
                        nullable_str = " (@Nullable)" if nullable else ""
                        print(f"    {field_type} {field_name}{nullable_str}")
                        fields.append((field_type, field_name, nullable))
                    
                    # Look for nested enums (but not classes)
                    enum_match = re.search(r'enum\s+(\w+)', current_line)
                    if enum_match and 'Padding' not in enum_match.group(1):
                        enum_name = enum_match.group(1)
                        print(f"    NESTED ENUM: {enum_name}")
                    
                    i += 1
                
                types.append({
                    'kind': type_kind,
                    'name': type_name,
                    'extends_implements': extends_implements,
                    'fields': fields
                })
        
        i += 1
    
    return types

if __name__ == "__main__":
    if len(sys.argv) != 2:
        print("Usage: python cs_type_extractor.py <path_to_Cs.java>")
        sys.exit(1)
    
    file_path = sys.argv[1]
    types = extract_type_info(file_path)