# CS-CPU-emulated

A small 16 bit CPU written in C#, you can change the clock speed via the public int clock number.

OPcode:
```
(N = null, A = A register, B = B register, T = All)
    (N) 0000 - NOP        No operations
    (A) 0001 - MMV        Memory move
    (T) 0010 - MOV        Register move
    (A) 0011 - LDA        Load A
    (B) 0100 - LDB        Load B
    (A) 0101 - WTA        Write to A
    (B) 0110 - WTB        Write to B
    (T) 0111 - ADD        Add A & B
    (T) 1000 - SUB        Sub A & B
    (T) 1001 - MUL        Mul A & B
    (N) 1010 - JMP        Jump
    (T) 1011 - CMP        Compare
    (T) 1100 - JNE        Jump not equal
    (T) 1101 - JIE        Jump if equal
    (N) 1110 - HLT        Halt
    (T) 1111 - CRB        Set value in B
```

Formatting:
```
    MMV:
    Address
    Address

    MOV:
    From register
    
    LDA & LDB:
    Address

    WTA & WTB: 
    Address

    JMP:
    Address

    JNE & JIE:
    Address
    
    *The rest are all single instruction*
```
Calling:
```
    print:
    <string>
  
    println:
    <string>

    printm:
    Address

    printlm:
    Address

    scanf:
    Address
```
