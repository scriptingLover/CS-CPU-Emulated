﻿using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CPU {
    public class CPU {
        public UInt16 A;
        public UInt16 B;
        public UInt16[] M = new UInt16[65536];

        public int clock = 1000000;

        public static void Main(string[] args) {
            CPU cpu = new CPU();

            bool show = false;
            Console.Write("Show Register and PC? (n / n): ");
            string nn = Console.ReadLine();

            Console.Clear();

            if (nn == "y" || nn == "Y")
                show = true;

            string[] x = File.ReadAllLines("input.txt");

            for (int i = 0; i < x.Length; i++) {
                x[i] = x[i].Replace("\t", "");
                x[i] = x[i].Replace("\"", "");
            }

            for (int i = 0; i < x.Length;) {
                int check = 0;
                bool trigger = true;

                string res;
                if (x[i] != "")
                    res = x[i].Substring(0, 1);
                else
                    res = "";

                if (res == ";" || x[i].Contains("global") || x[i].Contains("NULLcall")) {
                    trigger = false;
                    check = 1;
                }

                if (x[i] == "") {
                    trigger = false;
                    check = 1;
                }

                if (x[i] == "NOP" || x[i] == "nop") {
                    if (show == true)
                        Console.WriteLine("NOP");
                    check = 1;
                }

                if (x[i] == "MMV" || x[i] == "mmv") {
                    if (show == true)
                        Console.WriteLine("MMV");
                    UInt16 temp1 = Convert.ToUInt16(x[i + 1]);
                    UInt16 temp2 = Convert.ToUInt16(x[i + 2]);

                    cpu.M[temp2] = cpu.M[temp1];

                    i = i + 2;
                    check = 1;
                }

                if (x[i] == "MOV" || x[i] == "mov") {
                    if (show == true)
                        Console.WriteLine("MOV");
                    string temp = x[i + 1];

                    if (temp == "A")
                        cpu.B = cpu.A;
                    if (temp == "B")
                        cpu.A = cpu.B;

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "LDA" || x[i] == "lda") {
                    if (show == true)
                        Console.WriteLine("LDA");
                    UInt16 temp1 = Convert.ToUInt16(x[i + 1]);

                    cpu.M[temp1] = cpu.A;

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "LDB" || x[i] == "ldb") {
                    if (show == true)
                        Console.WriteLine("LDB");
                    UInt16 temp1 = Convert.ToUInt16(x[i + 1]);

                    cpu.M[temp1] = cpu.B;

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "WTA" || x[i] == "wta") {
                    if (show == true)
                        Console.WriteLine("WTA");
                    UInt16 temp1 = Convert.ToUInt16(x[i + 1]);

                    cpu.A = cpu.M[temp1];

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "WTB" || x[i] == "wtb") {
                    if (show == true)
                        Console.WriteLine("WTB");
                    UInt16 temp1 = Convert.ToUInt16(x[i + 1]);

                    cpu.B = cpu.M[temp1];

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "ADD" || x[i] == "add") {
                    if (show == true)
                        Console.WriteLine("ADD");
                    int temp = cpu.A + cpu.B;
                    try {
                        cpu.A = Convert.ToUInt16(temp);
                        check = 1;
                    }
                    catch (Exception e) {
                        cpu.A = (UInt16)(temp & 0xffff);
                        check = 1;
                    }
                }

                if (x[i] == "SUB" || x[i] == "sub") {
                    if (show == true)
                        Console.WriteLine("SUB");
                    try {
                        int temp = cpu.A - cpu.B;
                        cpu.A = Convert.ToUInt16(temp);
                    }
                    catch (Exception e) {
                        int temp2 = 65535 - (cpu.B - cpu.A) + 1;
                        cpu.A = Convert.ToUInt16(temp2);
                    }

                    check = 1;
                }

                if (x[i] == "MUL" || x[i] == "mul") {
                    if (show == true)
                        Console.WriteLine("MUL");
                    int temp = cpu.A * cpu.B;
                    try {
                        cpu.A = Convert.ToUInt16(temp);
                        check = 1;
                    }
                    catch (Exception e) {
                        cpu.A = (UInt16)(temp & 0xffff);
                        check = 1;
                    }
                }

                if (x[i] == "JMP" || x[i] == "jmp") {
                    if (show == true)
                        Console.WriteLine("JMP");

                    i = Convert.ToUInt16(x[i + 1]);
                    check = 1;
                }

                if (x[i] == "CMP" || x[i] == "cmp") {
                    if (show == true)
                        Console.WriteLine("CMP");
                    if (cpu.A == cpu.B)
                        cpu.A = 0;

                    if (cpu.A > cpu.B)
                        cpu.A = 1;

                    if (cpu.A < cpu.B)
                        cpu.A = 2;
                    check = 1;
                }

                if (x[i] == "JNE" || x[i] == "jne") {
                    if (show == true)
                        Console.WriteLine("JNE");

                    UInt16 temp;
                    try {
                        temp = Convert.ToUInt16(x[i + 1]);

                        if (cpu.A != cpu.B)
                            i = temp;

                        else
                            i = i + 1;

                        check = 1;
                    }
                    catch (Exception e) {
                        string temp3 = x[i + 1];

                        if (cpu.A != cpu.B) {
                            for (int t = 0; t < x.Length; t++) {
                                if (x[t].Contains(temp3) && x[t].Contains("global")) {
                                    i = t;
                                    check = 1;
                                    break;
                                }
                            }
                        }

                        else {
                            i = i + 1;
                            check = 1;
                        }
                    }
                }

                if (x[i] == "JIE" || x[i] == "jie") {
                    if (show == true)
                        Console.WriteLine("JIE");

                    UInt16 temp;
                    try {
                        temp = Convert.ToUInt16(x[i + 1]);

                        if (cpu.A == cpu.B)
                            i = temp;

                        else
                            i = i + 1;
                    }
                    catch (Exception e) {
                        string temp3 = x[i + 1];
                        int temp2 = 0;

                        if (cpu.A == cpu.B) {
                            for (int t = 0; t < x.Length; t++) {
                                if (x[t].Contains(temp3) && x[t].Contains("global")) {
                                    i = t;
                                    check = 1;
                                    break;
                                }
                            }
                        }

                        else {
                            i = i + 1;
                            check = 1;
                        }
                    }
                }

                if (x[i] == "HLT" || x[i] == "hlt") {
                    if (show == true)
                        Console.WriteLine("HLT");

                    if (show == true) {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("Register A: ");
                        Console.Write("Hex " + "{0:X4}", cpu.A);
                        Console.WriteLine(" | Decimal " + "{0:00000}", cpu.A);

                        Console.Write("Register B: ");
                        Console.Write("Hex " + "{0:X4}", cpu.B);
                        Console.WriteLine(" | Decimal " + "{0:00000}", cpu.B);

                        Console.Write("Memory ADR: ");
                        Console.Write("Hex " + "{0:X4}", i);
                        Console.WriteLine(" | Decimal " + "{0:00000}", i);

                        Console.Write("\n");

                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    while (true) ;
                }

                if (x[i] == "CRB" || x[i] == "crb") {
                    if (show == true)
                        Console.WriteLine("CRB");
                    UInt16 temp = Convert.ToUInt16(x[i + 1]);

                    cpu.B = temp;

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "call print") {
                    Console.Write(x[i + 1]);

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "call println") {
                    Console.WriteLine(x[i + 1]);

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "call printm") {
                    UInt16 temp1 = Convert.ToUInt16(x[i + 1]);
                    char temp2;

                    for (UInt16 t = temp1; cpu.M[t] != 0; t++) {
                        temp2 = Convert.ToChar(cpu.M[t]);
                        Console.Write(temp2);
                    }

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "call printlm") {
                    UInt16 temp1 = Convert.ToUInt16(x[i + 1]);
                    char temp2;

                    for (UInt16 t = temp1; cpu.M[t] != 0; t++) {
                        temp2 = Convert.ToChar(cpu.M[t]);
                        Console.Write(temp2);
                    }

                    Console.Write("\n");

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "call printi") {
                    Console.Write(cpu.M[Convert.ToInt32(x[i + 1])]);

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "call printli") {
                    Console.WriteLine(cpu.M[Convert.ToInt32(x[i + 1])]);

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "call scanf") {
                    string temp = Console.ReadLine();
                    UInt16 temp2 = Convert.ToUInt16(x[i + 1]);

                    byte[] asciiBytes = Encoding.ASCII.GetBytes(temp);
                    int[] asciiInts = Array.ConvertAll(asciiBytes, b => (int)b);

                    for (int t = 0; t < asciiInts.Length; t++) {
                        cpu.M[t + temp2] = Convert.ToUInt16(asciiInts[t]);
                    }

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "call iscanf") {
                    try {
                        UInt16 temp = Convert.ToUInt16(Console.ReadLine());
                        cpu.M[Convert.ToInt32(x[i + 1])] = temp;
                    }
                    catch (Exception e) {
                        cpu.M[Convert.ToInt32(x[i + 1])] = 0;
                    }

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "call itoa") {
                    string temp = Convert.ToString(cpu.M[Convert.ToUInt16(x[i + 1])]);

                    for (int t = 0; t < temp.Length; t++)
                        cpu.M[t + Convert.ToUInt16(x[i + 1])] = temp[t];

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "call atoi") {
                    int index = Convert.ToInt32(x[i + 1]);
                    try {
                        StringBuilder sb = new StringBuilder();
                        while (cpu.M[index] != 0) {
                            sb.Append((char)cpu.M[index]);
                            index++;
                        }
                        int temp = int.Parse(sb.ToString());

                        cpu.M[Convert.ToInt32(x[i + 1])] = Convert.ToUInt16(temp);
                    }
                    catch (Exception e) {
                        cpu.M[Convert.ToUInt16(x[i + 1])] = 0;
                    }

                    i = i + 1;
                    check = 1;
                }

                if (x[i] == "call func") {
                    string temp = x[i + 1];
                    int temp2 = 0;

                    for (int t = 0; t < x.Length; t++) {
                        if (x[t].Contains(temp) && !x[t].Contains("NULLcall")) {
                            temp2++;

                            if (temp2 == 2) {
                                i = t;
                                break;
                            }
                        }
                    }
                    check = 1;
                }

                if (x[i] == "call ascii") {
                    try {
                        UInt16 temp = Convert.ToUInt16(x[i + 1]);
                        string temp2 = x[i + 2];

                        for (int t = 0; t < temp2.Length; t++) {
                            cpu.M[t + Convert.ToUInt16(x[i + 1])] = temp2[t];
                        }

                        check = 1;
                    }
                    catch (Exception e) {
                        check = 0;
                    }

                    i = i + 2;
                }

                if (x[i] == "call strcmp") {
                    UInt16 temp1 = Convert.ToUInt16(x[i + 1]);
                    UInt16 temp2 = Convert.ToUInt16(x[i + 2]);

                    int index = temp1;
                    UInt16[] temp3 = new UInt16[65536];
                    UInt16[] temp4 = new UInt16[65536];

                    while (cpu.M[index] != 0) {
                        temp3[index] = cpu.M[index];
                        index++;
                    }

                    for (int t = 0; t < 5; t++) {
                        if (temp3[t] == 0)
                            Console.WriteLine("0");

                        Console.WriteLine(temp3[t]);
                    }

                    index = temp2;
                    while (cpu.M[index] != 0) {
                        temp4[index] = cpu.M[index];
                        index++;
                    }

                    for (int t = 0; t < 5; t++) {
                        if (temp4[t] == 0)
                            Console.WriteLine("0");

                        Console.WriteLine(temp4[t]);
                    }

                    bool eq = temp3.SequenceEqual(temp4);

                    if (eq == true)
                        cpu.A = 0;

                    else
                        cpu.A = 1;

                    i = i + 2;
                    check = 1;
                }

                i = i + 1;

                if (check == 0) {
                    Console.WriteLine("\nFalse assembly!");
                    Console.WriteLine("Program counter: " + i);
                    while (true) ;
                }

                if (show == true && trigger == true) {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Register A: ");
                    Console.Write("Hex " + "{0:X4}", cpu.A);
                    Console.WriteLine(" | Decimal " + "{0:00000}", cpu.A);

                    Console.Write("Register B: ");
                    Console.Write("Hex " + "{0:X4}", cpu.B);
                    Console.WriteLine(" | Decimal " + "{0:00000}", cpu.B);

                    Console.Write("Memory ADR: ");
                    Console.Write("Hex " + "{0:X4}", i);
                    Console.WriteLine(" | Decimal " + "{0:00000}", i);

                    Console.Write("\n");

                    Console.ForegroundColor = ConsoleColor.White;
                }

                if (trigger == true)
                    Thread.Sleep(1000 / cpu.clock);
            }
        }
    }
}
