// These are issued to Slaves by the Master

namespace 
Programmer
{
	public class 
	Constants
	{


		// Commands to SID
		
		public const int USBCMD_NULL			= 0x00;
		public const int USBCMD_INIT			= 0x01;
		public const int USBCMD_MASTER			= 0x02;
		public const int USBCMD_COMMAND			= 0x03;
		public const int USBCMD_BUS_READ		= 0x04;
		public const int USBCMD_SLAVE_COUNT		= 0x05;	// Do NOT use
		public const int USBCMD_LOADER_READ		= 0x06; // Do NOT use	
		public const int USBCMD_LOADER_ERASE	= 0x07;	// Do NOT use
		public const int USBCMD_LOADER_WRITE	= 0x08;	// Do NOT use
		public const int USBCMD_LOADER_CHECKSUM = 0x09;	// Do NOT use
		public const int USBCMD_DUT_RESET		= 0x0A;	// Do NOT use
		public const int USBCMD_DONGLE_RESET	= 0x0B;	
		public const int USBCMD_DONGLE_PING		= 0x0C;	// Do NOT use
		public const int USBCMD_BUS_START		= 0x06;	// V2.0 only
		public const int USBCMD_BUS_DATA		= 0x07; // V2.0 only	
		public const int USBCMD_BUS_POLL		= 0x08;	// V2.0 only
		public const int USBCMD_BUS_REVISION	= 0x0F;	// V2.0 only


		// !!! SID Revision 2.0+ ONLY !!!

		public const int USBCMD_STATE = 0x06;
		
		public const int USBCMD_BLM_READ	= 0xF0;
		public const int USBCMD_BLM_WRITE	= 0xF1;
		public const int USBCMD_BLM_MODE	= 0xF2;
		
		// Commands over the bus from SID to Slave
		
		public const int BUSCMD_NULL =		0x00;	// accept but do nothing, keeps the slave alive
		public const int BUSCMD_MODULE =	0x01;	// get slaves module name
		public const int BUSCMD_ENUM =		0x02;	// enumerate modules class information
		public const int BUSCMD_CLASS =		0x03;	// get sensor information
		public const int BUSCMD_NAME =		0x04;	// get sensor's name
		public const int BUSCMD_WRITE =		0x05;	// write to sensor
		public const int BUSCMD_READ =		0x06;	// read from sensor
		public const int BUSCMD_AUX =		0x07;	// read raw data from sensor
		public const int BUSCMD_CALIBRATE = 0x08;	// calibrate sensor
		public const int BUSCMD_BLINK =		0x09;	// on/off module's blink
		public const int BUSCMD_AR_DONE =	0x0A;	// The master's response to the slaves Command ping
		public const int BUSCMD_SUSPEND =	0x0B;	// The slave is put into an inactive mode


		// These next are supported ONLY by the master when in slave mode
		// !!! Do not use under any circumstances !!!
		
		public const int BUSCMD_FLASH_READ = 0x01;		// Reads 0 to 8 bytes from flash from page:address16
		public const int BUSCMD_FLASH_ERASE = 0x02;		// Erases one of 32 4K pages
		public const int BUSCMD_FLASH_WRITE = 0x03;		// data is written at page:adress16
		public const int BUSCMD_FLASH_CHECKSUM = 0x04;	// Returns internal flash checksum
		public const int BUSCMD_FLASH_RESET = 0x05;		// forces DUT to reset
	
		// SID to DUT in test mode
		
		public const int TCOP_MW  = 0x04;
		public const int TCOP_MR  = 0x05;
		public const int TCOP_IOW = 0x06;
		public const int TCOP_IOR = 0x07;

		public const int TC_INIT	= 0x81;
		public const int TC_POWER	= 0x82;
		public const int TC_ACQUIRE = 0x83;
		public const int TC_QUE		= 0x84;
		public const int TC_MR		= 0x85;
		public const int TC_MW		= 0x86;
		public const int TC_SDA		= 0x87;
		public const int TC_OP		= 0x88;

		public const int OP_READ_BLOCK = 0x01;
		public const int OP_WRITE_BLOCK = 0x02;
		public const int OP_ERASE_BLOCK = 0x03;
		public const int OP_PROTECT = 0x04;
		public const int OP_ERASE_ALL = 0x05;
		public const int OP_TABLE_READ = 0x06;
		public const int OP_CHECKSUM = 0x07;
		public const int OP_CALIBRATE = 0x08;
}
}
