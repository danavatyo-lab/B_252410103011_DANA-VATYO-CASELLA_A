using System;

class Sekolah
{
    public string idSekolah, namaSekolah;
    public int jumlahPaketDiterima;

    public Sekolah(string id, string nama, int jumlah)
    {
        idSekolah = id;
        namaSekolah = nama;
        jumlahPaketDiterima = jumlah;
    }

    public void TampilInfo()
    {
        Console.WriteLine(idSekolah + " | " + namaSekolah + " | Paket diterima: " + jumlahPaketDiterima);
    }
}

abstract class PaketMBG
{
    private string idPaket, namaPaket;
    private int stokDapur;

    public string IdPaket { get { return idPaket; } set { idPaket = value; } }
    public string NamaPaket { get { return namaPaket; } set { namaPaket = value; } }
    public int StokDapur
    {
        get { return stokDapur; }
        set { if (value >= 0) stokDapur = value; }
    }

    public PaketMBG(string id, string nama, int stok)
    {
        idPaket = id;
        namaPaket = nama;
        StokDapur = stok;
    }

    public void KurangiStok(int jumlah)
    {
        if (jumlah > 0 && jumlah <= stokDapur) stokDapur -= jumlah;
    }

    public abstract int HitungBiaya(int jumlah);

    public virtual void TampilInfo()
    {
        Console.WriteLine(idPaket + " | " + namaPaket + " | Stok: " + stokDapur);
    }
}

class PaketReguler : PaketMBG
{
    public PaketReguler(string id, string nama, int stok) : base(id, nama, stok) { }
    public override int HitungBiaya(int jumlah) { return jumlah * 15000; }
}

class PaketKhusus : PaketMBG
{
    public string jenisDiet;

    public PaketKhusus(string id, string nama, int stok, string diet) : base(id, nama, stok)
    {
        jenisDiet = diet;
    }

    public override int HitungBiaya(int jumlah) { return jumlah * 20000; }

    public override void TampilInfo()
    {
        base.TampilInfo();
        Console.WriteLine("Jenis diet: " + jenisDiet);
    }
}

interface DapatDidistribusikan
{
    void KirimPaket();
}

class DistribusiMBG : DapatDidistribusikan
{
    public Sekolah sekolah;
    public PaketMBG paket;
    public int jumlah;

    public DistribusiMBG(Sekolah s, PaketMBG p, int j)
    {
        sekolah = s;
        paket = p;
        jumlah = j;
    }

    public void KirimPaket()
    {
        if (jumlah <= 0)
            Console.WriteLine("Gagal! Jumlah paket harus lebih dari 0.");
        else if (jumlah > paket.StokDapur)
            Console.WriteLine("Gagal! Jumlah paket melebihi stok dapur.");
        else
        {
            paket.KurangiStok(jumlah);
            sekolah.jumlahPaketDiterima += jumlah;
            Console.WriteLine("Distribusi MBG berhasil! Sebanyak " + jumlah + " paket makanan telah dikirim ke sekolah " + sekolah.namaSekolah);
        }
    }
}

class Program
{
    static void Main()
    {
        Sekolah s1 = new Sekolah("S001", "SDN Jember Lor 1", 0);
        Sekolah s2 = new Sekolah("S002", "SMPN 3 Jember", 50);

        PaketMBG reguler = new PaketReguler("P001", "Paket Reguler", 100);
        PaketMBG khusus = new PaketKhusus("P002", "Paket Khusus", 40, "Rendah Gula");

        Console.WriteLine("Biaya 10 reguler: Rp" + reguler.HitungBiaya(10));
        Console.WriteLine("Biaya 10 khusus : Rp" + khusus.HitungBiaya(10));

        new DistribusiMBG(s1, reguler, 30).KirimPaket();
        new DistribusiMBG(s2, khusus, 100).KirimPaket();
        new DistribusiMBG(s2, reguler, 0).KirimPaket();

        Console.ReadLine();
    }
}