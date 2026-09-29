# Native Director fixtures

Fixtures are grouped by Director version (`d7`, `d8`, etc.), with filenames describing their content. The `empty.dir` and `empty.dcr` pairs were generated on Windows with the 32-bit `dirdrv` host and the installed Director DLLs/Xtras.

| Folder | Director installation | DLL file version |
| ------ | --------------------- | ---------------- |
| `d7`   | Director 7            | 7.0.2r85         |
| `d8`   | Director 8            | 8.0r178          |
| `d85`  | Director 8.5          | 8.5.1r102        |
| `d9`   | Director MX           | 8.5.1r104        |
| `d10`  | Director MX 2004      | 10.1r11          |
| `d11`  | Director 11.0.3       | 11.0.3r466       |
| `d115` | Director 11.5         | 11.5r593         |
| `d12`  | Director 12           | 12.0r111         |

To regenerate a pair, build `dirdrv` for `i686-pc-windows-msvc` and run:

```powershell
& $dirdrv empty --engine-dir $directorInstall --out empty.dir --afterburn
```