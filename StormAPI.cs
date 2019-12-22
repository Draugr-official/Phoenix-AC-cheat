using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Threading;

public class StormClient

{

    public static bool debugMode;

    private IntPtr baseAddress;

    private ProcessModule processModule;

    private Process[] mainProcess;

    private IntPtr processHandle;

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint dwSize, uint lpNumberOfBytesRead);

    [DllImport("kernel32.dll")]
    private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, uint lpNumberOfBytesWritten);

    [DllImport("kernel32.dll")]
    private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll")]
    private static extern bool CloseHandle(IntPtr hObject);

    [DllImport("kernel32.dll")]
    private static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

    [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
    private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

    public string processName { get; set; }
    /// <summary>
    /// <para>Gets baseaddress from the first process in a client process</para>
    /// </summary>
    public long getBaseAddress
    {
        get
        {
            this.baseAddress = (IntPtr)0;
            this.processModule = this.mainProcess[0].MainModule;
            this.baseAddress = this.processModule.BaseAddress;
            return (long)this.baseAddress;
        }
    }

    public StormClient()
    {
    }
    /// <summary>
    /// <para>Bind the program to a process (StormClient strm = new StormClient("a_client"))</para>
    /// </summary>
    public StormClient(string pProcessName)
    {
        this.processName = pProcessName;
    }
    /// <summary>
    /// <para>Check if process is valid or running</para>
    /// </summary>
    public bool CheckProcess()
    {
        if (this.processName == null)
        {
            MessageBox.Show("Process name is undefined");
            return false;
        }
        this.mainProcess = Process.GetProcessesByName(this.processName);
        if (this.mainProcess.Length == 0)
        {
            this.ErrorProcessNotFound(this.processName);
            return false;
        }
        this.processHandle = StormClient.OpenProcess(2035711u, false, this.mainProcess[0].Id);
        if (this.processHandle == IntPtr.Zero)
        {
            this.ErrorProcessNotFound(this.processName);
            return false;
        }
        return true;
    }
    /// <summary>
    /// <para>BaseOffset is used to find the LocalPlayer, OffsetToChange is used to find the value you want to change, Value is used as the changer value (selected offset value will get overwritten with Value)</para>
    /// </summary>
    public void ChangeAddressValue(int BaseOffset, int OffsetToChange, int Value)
    {
        int baseaddress = ReadInt32((IntPtr)BaseOffset);
        int address = baseaddress + OffsetToChange;
        WriteInt32((IntPtr)address, Value);
    }
    /// <summary>
    /// <para>Returns Int32 of a destined offset</para>
    /// </summary>
    public int GetAddressValue(int BaseOffset, int OffsetToChange)
    {
        int baseaddress = ReadInt32((IntPtr)BaseOffset);
        int address = baseaddress + OffsetToChange;
        return ReadInt32((IntPtr)address);
    }
    /// <summary>
    /// <para>Returns Int32 of a destined offset / if offset was not found or was not assigned a value, will return 0</para>
    /// </summary>
    public int TryGetAddressValue(int BaseOffset, int OffsetToRead)
    {
        try
        {
            int baseaddress = ReadInt32((IntPtr)BaseOffset);
            int address = baseaddress + OffsetToRead;
            return ReadInt32((IntPtr)address);
        }
        catch(Exception)
        {
            return 0;
        }
    }
    /// <summary>
    /// <para>Reads Byte Array(s) from a destined offset</para>
    /// </summary>
    public byte[] ReadByteArray(IntPtr pOffset, uint pSize)
    {
        if (this.processHandle == IntPtr.Zero)
        {
            this.CheckProcess();
        }
        byte[] result;
        try
        {
            uint flNewProtect;
            StormClient.VirtualProtectEx(this.processHandle, pOffset, (UIntPtr)pSize, 4u, out flNewProtect);
            byte[] array = new byte[pSize];
            StormClient.ReadProcessMemory(this.processHandle, pOffset, array, pSize, 0u);
            StormClient.VirtualProtectEx(this.processHandle, pOffset, (UIntPtr)pSize, flNewProtect, out flNewProtect);
            result = array;
        }
        catch (Exception ex)
        {
            if (StormClient.debugMode)
            {
                Console.WriteLine("Read As ByteArray" + ex.ToString());
            }
            result = new byte[1];
        }
        return result;
    }
    /// <summary>
    /// <para>Reads Int32(s) from a destined offset</para>
    /// </summary>
    public int ReadInt32(IntPtr pOffset)
    {
        if (this.processHandle == IntPtr.Zero)
        {
            this.CheckProcess();
        }
        int result;
        try
        {
            result = BitConverter.ToInt32(this.ReadByteArray(pOffset, 4u), 0);
        }
        catch (Exception ex)
        {
            if (StormClient.debugMode)
            {
                Console.WriteLine("Read As Int32" + ex.ToString());
            }
            result = 0;
        }
        return result;
    }
    
    /// <summary>
    /// <para>Writes a byte array to a destined Offset</para>
    /// </summary>
    public bool WriteByteArray(IntPtr pOffset, byte[] pBytes)
    {
        if (this.processHandle == IntPtr.Zero)
        {
            this.CheckProcess();
        }
        bool result;
        try
        {
            uint flNewProtect;
            StormClient.VirtualProtectEx(this.processHandle, pOffset, (UIntPtr)((ulong)((long)pBytes.Length)), 4u, out flNewProtect);
            bool flag = StormClient.WriteProcessMemory(this.processHandle, pOffset, pBytes, (uint)pBytes.Length, 0u);
            StormClient.VirtualProtectEx(this.processHandle, pOffset, (UIntPtr)((ulong)((long)pBytes.Length)), flNewProtect, out flNewProtect);
            result = flag;
        }
        catch (Exception ex)
        {
            if (StormClient.debugMode)
            {
                Console.WriteLine("Write As ByteArray" + ex.ToString());
            }
            result = false;
        }
        return result;
    }
    
    /// <summary>
    /// <para>Writes (a) Int32 to a destined offset</para>
    /// </summary>
    public bool WriteInt32(IntPtr pOffset, int pData)
    {
        if (this.processHandle == IntPtr.Zero)
        {
            this.CheckProcess();
        }
        bool result;
        try
        {
            result = this.WriteByteArray(pOffset, BitConverter.GetBytes(pData));
        }
        catch (Exception ex)
        {
            if (StormClient.debugMode)
            {
                Console.WriteLine("Write As Int32" + ex.ToString());
            }
            result = false;
        }
        return result;
    }
    
    /// <summary>
    /// <para>Errors upon proccess not found</para>
    /// </summary>
    private void ErrorProcessNotFound(string pProcessName)
    {
        MessageBox.Show(this.processName + " is not running / was not found", "Process Not Found", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }
}
