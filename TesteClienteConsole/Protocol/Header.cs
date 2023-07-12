namespace Protocol;
/*  Estrutura da verificação do byte para bitmasking:
    1º bit : timestamp
    2º bit : Resposta

*/

public class Header
{
    public int extensions { get; set; }
    public ushort msgID { get; set; }
    public ushort msgIdOrigin { get; set; }
    public byte bmask { get; set; } = 0b_00000000;
    const int headerSize = 8;
    public ushort msgSize { get; set; } = 0;
    // Convert char to byte
    public byte signByte = 88; //valor decimal que representa o X na tabela ASCII

    public Header()
    {
        
    }
    public void RecvHeader(byte[] _header, Session _session)
    {
        if((_header[1] & (1 << 1)) != 0)
        {
           RecvExtHeader(_header,_session);
        }
        
        //msgSize =(ushort)((_header[1]<<8) | _header[2]);
        msgSize = (ushort)(_header[2]);

        msgSize <<= 8;
        msgSize |= _header[3];
    }
    public void RecvExtHeader(byte[] _header, Session _session)
    {   
        //para já, a extensão será de 8 bytes por só transpotar o timestamp, no futuro,
        //poderá ser variavel, somando os bytes necessários consoante cada
        //bit ligado no bitmasking.
        byte[] extHeader = new byte[8];
        _session.Recv(extHeader, (ushort)extHeader.Length);
        //traduzido logo para datetime, uma vez que o unico conteudo será o timestamp.
        DateTime myDateTime = DateTime.FromBinary(BitConverter.ToInt64(extHeader));
    }

    public void isResponse()
    {
        bmask|=0b_00000010;
    }

    public byte[] isExtended()
    {
        bmask|=0b_00000001;
        DateTime ts = DateTime.Now;
        byte[] timestamp = BitConverter.GetBytes(ts.Ticks);
        return timestamp;
    }

    public byte[] cookHeader()
        {
            byte[] header = new byte[8];
            header[0] = signByte;
            //bitmask
            header[1] = bmask;
            //tamanho da mensagem a mandar
            header[2] = (byte)(msgSize >> 8);
            header[3] = (byte)(msgSize & 255);
            //Id da mensagem
            header[4] = (byte)(msgID >> 24);
            header[5] = (byte)((msgID >> 16) & 0xFF);
            header[6] = (byte)((msgID >> 8) & 0xFF);
            header[7] = (byte)(msgID & 0xFF);


            return header;
        }
}