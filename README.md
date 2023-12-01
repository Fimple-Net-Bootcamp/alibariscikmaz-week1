# OOP Prensipleri ile Kütüphane Sistemi
Projemde bir kütüphane sistemini simüle etmek için nesne tabanlı programla prensipleri kullanılarak farklı sınıflar oluşturulmuştur.
- IPrintable: Özelliklerini yazdırmak isteyebileceğimiz sınınfların kalıtım alması için oluşturulmuş bir arayüz sınıfıdır.
- Literature: Book sınıfının kalıtım alması için oluşturulmuş üst sınıftır. Kitapta da bulunacak Name, Author, Publication Year özelliklerini içerir.
- Book: Literature sınıfından belirtilen özellikleri, IPrintable arayüzünden de printProperties methodunu kalıtım alan, aynı zamanda içinde ID, isBorrowed ve DaysOfBorrow özelliklerini ve toplam oluşturulmuş kitap sayısını static olarak içeren kitap sınıfıdır. Yapıcı methodunda toplam kitap sayısı artmakta, özellikleri doldurulmaktadır.
- Member: IPrintable arayüzünden printProperties methodunu kalıtım alan, içinde Name, Surname, ID özelliklerini ve toplam oluşturulmuş kitap sayısını static olarak, ödünç alınan kitapları da liste olarak BorrowedBooks listesinde tutan kütüphane üyesi sınıfıdır. Yapıcı methodunda toplam üye sayısı artmakta, özellikleri doldurulmaktadır.
- Library: IPrintable arayüzünden printProperties methodunu kalıtım alan, içinde liste olarak BookInventory (Kütüphaneye kayıtlı kitaplar) ve RegisteredMembers (Kütüphaneye kayıtlı üyeler) tutan ve kütüphane işlemleriyle alakalı methodlar tutan sınıftır.
  
  -addBookToInventory methoduyla kitap objesini alarak, eğer kütüphanede halihazırda kayıtlı değilse kütüphaneye ekler.
  -removeBookFromInventory methoduyla kitap objesini alarak, eğer kütüphanede halihazırda kayıtlıysa kütüphaneden siler.
  -registerMemberToLibrary methoduyla üye objesini alarak, eğer kütüphanede halihazırda kayıtlı değilse kütüphaneye ekler.
  -registerMemberToLibrary methoduyla üye objesini alarak, eğer kütüphanede halihazırda kayıtlıysa kütüphaneden siler.
  -borrowBook methodunun süreli, süresiz ödünç alma ve kitap adı ve kitap id'siyle ödünç alma gibi farklı varyasyonları bulunmakta, polymorphism uygulanmaktadır. Uygun koşullarda kitabın ödünç alınmasını sağlar.
  -returnBook methodu uygun koşullarda kitabın iade edilmesini sağlar.

  Kodun sonunda test için belli objeler oluşturulmuş, methodlar denenmiştir.
