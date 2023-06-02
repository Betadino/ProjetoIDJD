namespace Protocol;
/*  Estrutura da verificação do byte para bitmasking:
    1º bit : timestamp
    2º bit : Resposta

*/

public class  Header
{
    private int extensions { get; set; }
    private Int32 msgID { get; set; } 
    private byte bmask { get; set; }


    private Int16 msgSize  { get; set;} 
    // Convert char to byte
    private byte signByte = 88; //valor decimal que representa o X na tabela ASCII

    public Header()
    {
        bmask = 0b_00000000;
        msgSize=0;
    }
  

    //liga o 1º bit
    public byte enableTimeStamp()
    {   
        
        byte timestamp = (byte)DateTime.now;

            bmask |= 0b_00000001;
            ++extensions;

        return timestamp;
    }

    // liga o 2º bit
    public byte enableResponse()
    {   
      bmask|=0b_00000010;
    }


    public void setSize(Int16 size)
    {
        this.msgSize = size;
    }

    public void setID(Int32 msgID)
    {
        this.msgID = msgID; 
    }


    



    /* public Header( byte mask, Int32 msize)
     {
         bmask=mask;
         msgSize = (Int16)msg.Length;
         byte[] header =new byte[8+extensions];
         sign = 'X';
         header[0] = signByte;
         //bitmask
         header[1] = bmask;
         //tamanho da mensagem a mandar
         header[2] = (byte)(msgSize >> 8);
         header[3] = (byte)(msgSize & 255);
         //Id da mensagem
         header[4] = (byte) (msgID >> 24);
         header[5] = (byte)((msgID >> 16) & 0xFF);
         header[6] = (byte)((msgID >> 8) & 0xFF);
         header[7] = (byte) (msgID & 0xFF);
     }*/

}

