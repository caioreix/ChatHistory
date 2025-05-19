#!/bin/bash

# Check if a file was provided
if [ $# -eq 0 ]; then
    echo "Usage: drag and drop the chat.history file onto this script or pass it as an argument."
    exit 1
fi

input="$1"
output="$input.converted"

> "$output" # Clear the output file

url_escape() {
    # Convert each unsafe character to %XX
    # Use xxd to get bytes in hexadecimal format
    echo -n "$1" | xxd -p -c1 | while read c; do
        case "$c" in
            2d|2e|5f|7e|30|31|32|33|34|35|36|37|38|39|41|42|43|44|45|46|47|48|49|4a|4b|4c|4d|4e|4f|50|51|52|53|54|55|56|57|58|59|5a|61|62|63|64|65|66|67|68|69|6a|6b|6c|6d|6e|6f|70|71|72|73|74|75|76|77|78|79|7a)
                # Safe characters: - . _ ~ 0-9 A-Z a-z
                printf "\\x$c"
                ;;
            *)
                printf '%%%s' "$c"
                ;;
        esac
    done
}

while IFS= read -r line; do
    decoded=$(echo "$line" | base64 -d 2>/dev/null)
    url_escaped=$(url_escape "$decoded")
    echo "$url_escaped" >> "$output"
done < "$input"

echo "Conversion completed: $output"
