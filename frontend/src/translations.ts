export type TranslationType = {
  searchTitle: string;
  brand: string;
  model: string;
  modelCode: string;
  eg: string;
  bodyType: string;
  fuel: string;
  minPrice: string;
  maxPrice: string;
  Price: string;
  yearFrom: string;
  yearTo: string;
  chooseLocation: string;
  locations: string;
  moreSearch: string;
  search: string;
  homepage: string;
  allCar: string;
  cars: string;
  services: string;
  contact: string;
  login: string;
  footer: string;
  noResults: string;
  details: string;
  price: string;
  year: string;
  type: string;
  fuelType: string;
  selectLocation: string;
  complete: string;
  loginTitle: string;
  username: string;
  password: string;
  register: string;
  switchToRegister: string;
  switchToLogin: string;
  myProfile: string;
  myMessages: string;
  logout: string;
  advertisement: string;
  advertisementTitle: string;
  registerTitle: string;
  email: string;
  registrationSuccess: string;
  loginSuccess: string;
  loginError: string;
  footer_contact: string;
  footer_company: string;
  footer_address: string;
  footer_tax: string;
  footer_follow_us: string;
  footer_facebook: string;
  footer_instagram: string;
  footer_links: string;
  footer_about: string;
  footer_contact_page: string;
  footer_privacy: string;
  footer_rights: string;
  view_cars: string;
  name: string;
  nameKanji: string;
  phoneNumber: string;
  successLogin: string;
  successRegister: string;
  error: string;
  color: string;
  langu: string;
  postAd: string;
  successMessage: string;
  uploadImage: string;
  description: string;
  selectBrand: string;
  selectModel: string;
  mileage: string;
  dealership: string;
  closed: string;
  searchResult: string;
  welcomeText: string;
  searchCars: string;
  viewCars: string;
  drive: string;
  engineSize: string;
  mileageNum: string;
  savedCars: string;
  noSavedCars: string;
  savedCarsHome: string;
  noMessage: string;
  sender: string;
  loginToBook: string;
  chooseDate: string;
  loginForData: string;
  book: string;
  backHome: string;
    myData:string;
    save:string;
    viewDetails: string;
    chooseBrand: string;
    noModel: string;
    chooseModel: string;
    chooseBodyType: string;
    chooseFuel: string;
    chooseDrivetrain: string;
    chooseColor: string;
    hungarian: string;
  english: string;
  japanese: string;
  Year: string;
  /* */
  errorLoadingCar: string;
  loginToSave: string;
  errorNoUser: string;
  carSaved: string;
  errorSavingCar: string;
  noPastDate: string;
  alreadyBookedFirst: string;
  alreadyBookedSecond: string;
  doneBookFirst: string;
  doneBookSecond: string;
  errorBooking: string;
  loading: string;
  back: string;
  noImage: string;
  location: string;
  noDetails: string;
  appointment: string;
  mon: string;
  tue: string;
  wed: string;
  thu: string;
  fri: string;
  sat: string;
  sun: string;
  time: string;
  cancel: string;
  change: string;
  selectDate: string;
  saveCarErrorNoAuth: string;
  saveCarErrorNoUser: string;
  saveCarSuccess: string;
  saveCarError: string;
  manufacturingStartYear: string;
  manufacturingEndYear: string;
  passengerCount: string;
  dimensions: string;
  weight: string;
  transmission: string;
  drivetrain: string;
  vin: string;
  address: string;
  capacity: string;
  km: string;
  appointmentDate: string;
  branch: string;
  telephone: string;
  mass: string;
  licenseplate: string;
  repairHistory: string;
  yes: string;
  no: string;
  motExpiry: string;
  isSmoking: string;
};


export const translations = {
  en: {
    searchTitle: "Car Search",
    brand: "Brand:",
    model: "Model:",
    modelCode: "Model code:",
    eg: "Eg.",
    bodyType: "Body Type:",
    fuel: "Fuel:",
    minPrice: "Min Price:",
    maxPrice: "Max Price:",
    Price: "Price:",
    yearFrom: "Year From:",
    yearTo: "Year To:",
    chooseLocation: "Search Location",
    moreSearch: "More search",
    search: "Search",
    homepage: "Home",
    locations: "Locations",
    allCar: "All Cars",
    cars: "Cars",
    services: "Services",
    contact: "Contact",
    login: "Login",
    footer: "© 2025 Car Dealership. All rights reserved.",
    noResults: "No results match your search criteria.",
    details: "Details",
    price: "Price:",
    year: "Year:",
    type: "Body Type:",
    fuelType: "Fuel:",
    selectLocation: "Select a location",
    complete: "Complete",
    loginTitle: "Login",
    username: "Username",
    password: "Password",
    register: "Register",
    switchToRegister: "Switch to Register",
    switchToLogin: "Switch to Login",
    myProfile: "My Profile",
    myMessages: "My Messages",
    logout: "Logout",
    advertisement: "Post an Advertisement",
    advertisementTitle: "Post a Car Advertisement",
    registerTitle: "Registration",
    email: "Email",
    registrationSuccess: "Registration successful! Please log in now.",
    loginSuccess: "Login successful!",
    loginError: "Invalid username or password.",
    footer_contact: "Contact",
    footer_company: "Premium Cars Co., Ltd.",
    footer_address: "1-1 Chiyoda, Chiyoda-ku, Tokyo 100-0001",
    footer_tax: "Tax Number: 12345678-1-12",
    footer_follow_us: "Follow us",
    footer_facebook: "Facebook",
    footer_instagram: "Instagram",
    footer_links: "Useful Links",
    footer_about: "About Us",
    footer_contact_page: "Contact",
    footer_privacy: "Privacy Policy",
    footer_rights: "© 2025 Premium Cars Co., Ltd. All rights reserved.\n" +
        "Everything found here is strictly for demonstration purposes.",
    view_cars: "View cars",
    name: "Name",
    nameKanji: "Name (Kanji)",
    phoneNumber: "Phone Number",
    successLogin: "Login successful!",
    successRegister: "Registration successful! Please log in.",
    error: "Invalid credentials.",
    color: "Color:",
    langu: "Lang",
    postAd: "Post Ad",
    successMessage: "Success Ad Post",
    uploadImage: "Upload Image",
    description: "Description",
    selectBrand: "Select Brand",
    selectModel: "Select Model",
    mileage: "Mileage:",
    dealership: "Car Dealership",
    closed: "Close",
    searchResult: "Search Result",
    welcomeText: "Welcome in the Car Dealership!",
    searchCars: "Search between the new and used cars...",
    viewCars: "View Cars",
    drive:"Drive:",
    engineSize: "Engine Capacity:",
    mileageNum: "Current Mileage:",
    savedCars: "Saved Cars",
    noSavedCars: "No saved cars",
    savedCarsHome:"Saved Cars",
    noMessage: "No messages",
    sender: "Sender:",
    loginForData: "Please login for looking at your profil",
    myData:"My Data",
    save:"Save",
    viewDetails: "View Details",
    chooseBrand: "Choose brand",
    noModel: "No model found",
    chooseModel: "Choose a model",
    chooseBodyType: "Choose body type",
    chooseFuel: "Choose fuel",
    chooseDrivetrain: "Choose drivetrain",
    chooseColor: "Choose color",
    hungarian: "Hungarian",
  english: "English",
  japanese: "Japanese",
  Year: "Year",
    mass: "Mass",
  // English translations
  errorLoadingCar: "Error loading car data",
  loginToSave: "Please log in to save this car!",
  errorNoUser: "Error: No user is logged in!",
  carSaved: "Car successfully saved!",
  errorSavingCar: "Error occurred while saving the car!",
  loginToBook: "Please log in to book an appointment!",
  chooseDate: "Please select a date for booking!",
  noPastDate: "You cannot select a date in the past!",
  alreadyBookedFirst: "You already have an appointment on",
  alreadyBookedSecond: "You must cancel your existing appointment first.",
  doneBookFirst: "Booking successful!",
  doneBookSecond: "We look forward to seeing you at our dealership!",
  errorBooking: "An error occurred during booking. Please try again later!",
  loading: "Loading...",
  back: "Back",
  noImage: "No image",
  location: "Location",
  noDetails: "No detailed description available.",
  appointment: "Book an Appointment",
  mon: "M",
  tue: "T",
  wed: "W",
  thu: "T",
  fri: "F",
  sat: "S",
  sun: "S",
  time: "Time",
  cancel: "Cancel",
  change: "Change",
  selectDate: "Select date",
    saveCarErrorNoAuth: "Log in to save the car!",
    saveCarErrorNoUser: "Error: No logged-in user!",
    saveCarSuccess: "Car successfully saved!",
    saveCarError: "An error occurred while saving the car!",
    passengerCount: "Passenger Count",
    dimensions: "Dimensions",
    weight: "Weight",
    transmission: "Transmission",
    drivetrain: "Drivetrain",
    vin: "VIN",
    address: "Address",
    capacity: "Capacity",
    km: "Kilometers",
    appointmentDate: "Appointment Date",
    branch: "Branch",
    telephone: "Telephone",
    manufacturingStartYear: "Manufacturing Start Year",
    manufacturingEndYear: "Manufacturing End Year",
    licenseplate: "License Plate",
    repairHistory: "Repair History",
    yes: "Yes",
    no: "No",
    motExpiry: "MOT Expiry",
    isSmoking: "Smoking vehicle",
  },
  hu: {
    searchTitle: "Autókereső",
    brand: "Márka:",
    model: "Modell:",
    modelCode: "Típuskód:",
    eg: "Pl.",
    bodyType: "Kivitel:",
    fuel: "Üzemanyag:",
    minPrice: "Min ár:",
    maxPrice: "Max ár:",
    Price: "Ár:",
    yearFrom: "Év - tól:",
    yearTo: "Év - ig:",
    chooseLocation: "Helyszín kiválasztása",
    moreSearch: "Részletes keresés",
    search: "Keresés",
    homepage: "Kezdőlap",
    cars: "Autók",
    allCar: "Összes autó",
    services: "Szolgáltatások",
    contact: "Kapcsolat",
    login: "Bejelentkezés",
    footer: "© 2025 Autókereskedés. Minden jog fenntartva.",
    noResults: "Nincs találat a keresési feltételekre.",
    details: "Részletek",
    price: "Ár:",
    year: "Évjárat:",
    type: "Kivitel:",
    fuelType: "Üzemanyag:",
    selectLocation: "Válassza ki a helyszínt",
    complete: "Kész",
    loginTitle: "Bejelentkezés",
    username: "Felhasználónév",
    password: "Jelszó",
    register: "Regisztráció",
    locations: "Telepelyek",
    switchToRegister: "Váltás regisztrációra",
    switchToLogin: "Váltás bejelentkezésre",
    myProfile: "Profilom",
    myMessages: "Üzeneteim",
    logout: "Kijelentkezés",
    advertisement: "Hirdetésfeladás",
    advertisementTitle: "Autó hirdetés feladása",
    footer_contact: "Kapcsolat",
    footer_company: "Premium Autok Kft. (プレミアムカー株式会社)",
    footer_address: "Japán, 100-0001 Tokió Csijóda-ku Csijóda 1-1",
    footer_tax: "Adószám: 12345678-1-12",
    footer_follow_us: "Kövess minket",
    footer_facebook: "Facebook",
    footer_instagram: "Instagram",
    footer_links: "Hasznos linkek",
    footer_about: "Rólunk",
    footer_contact_page: "Kapcsolat",
    footer_privacy: "Adatvédelmi irányelvek",
    footer_rights: "© 2025 プレミアムカー Kft. Minden jog fenntartva.\n" +
        "Minden, ami itt található, kizárólag bemutató célokra van.",
    registerTitle: "Regisztráció",
    email: "E-mail",
    registrationSuccess: "Sikeres regisztráció! Most jelentkezz be.",
    loginSuccess: "Sikeres bejelentkezés!",
    loginError: "Hibás felhasználónév vagy jelszó.",
    view_cars: "Autók megtekintése",
    name: "Név",
    nameKanji: "Név (Kanji)",
    phoneNumber: "Telefonszám",
    successLogin: "Sikeres bejelentkezés!",
    successRegister: "Sikeres regisztráció! Kérjük, jelentkezzen be.",
    error: "Hibás adatok.",
    postAd: "Hirdetés feladás",
    description: "Leírás:",
    mileage: "Km:",
    color: "Szín:",
    langu:"Nyelv:",
    successMessage: "Sikeres hirdetés feladás",
    uploadImage: "Képfeltöltés:",
    selectBrand: "Márka:",
    selectModel: "Model:",
    dealership: "Autókereskedés",
    closed: "Bezárás",
    searchResult: "Keresési eredmények",
    welcomeText: "Üdvözlünk az Autókereskedésben!",
    searchCars: "Válogass kedvedre a használt és új autók közül...",
    viewCars: "Autók megtekintése",
    drive:"Meghajtás:",
    engineSize: "Motor méret:",
    mileageNum: "Km állás:",
    savedCars: "Mentett autóim",
    savedCarsHome: "Mentett autóim",
    noSavedCars: "Nincs mentett autód",
    noMessage: "Nincs megjeleníthető üzenet.",
    sender: "Feladó:",
    loginForData: "Kérjük, jelentkezz be a profil megtekintéséhez!",
    myData:"Adataim",
    save:"Mentés",
    viewDetails: "Részltetes leírás",
    chooseBrand: "Válassz márkát",
    noModel: "Nincs elérhető modell",
    chooseModel: "Válassz modellt",
    chooseBodyType: "Válassz kivitelt",
    chooseFuel: "Válassz üzemanyagot",
    chooseDrivetrain: "Válassz meghajtást",
    chooseColor: "Válassz színt",
    book: "Foglalás",
    backHome: "Vissza",
    hungarian: "Magyar",
  english: "Angol",
  japanese: "Japán",
  Year: "Év:",
  errorLoadingCar: "Hiba az autó adatainak betöltésekor",
  loginToSave: "Jelentkezz be az autó mentéséhez!",
  errorNoUser: "Hiba: Nincs bejelentkezett felhasználó!",
  carSaved: "Autó sikeresen mentve!",
  errorSavingCar: "Hiba történt az autó mentésekor!",
  loginToBook: "Jelentkezz be időpontfoglaláshoz!",
  chooseDate: "Válassz időpontot a foglaláshoz!",
  noPastDate: "Nem választhatsz múltbeli időpontot!",
  alreadyBookedFirst: "Már van foglalásod",
  alreadyBookedSecond: "Előbb törölnöd kell a meglévő időpontot.",
  doneBookFirst: "Sikeres foglalás!",
  doneBookSecond: "Várunk a szalonunkban!",
  errorBooking: "Hiba történt a foglalás során. Kérjük, próbáld újra később!",
  loading: "Betöltés...",
  back: "Vissza",
  noImage: "Nincs kép",
  location: "Telephely",
  noDetails: "Nincs részletes leírás.",
  appointment: "Időpontfoglalás",
    mass: "Tömeg",
  mon: "H",
  tue: "K",
  wed: "SZe",
  thu: "CS",
  fri: "P",
  sat: "SZo",
  sun: "V",
  time: "Időpont",
  cancel: "Mégse",
  change: "Módosítás",
  selectDate: "Időpont kiválasztása",
    saveCarErrorNoAuth: "Jelentkezz be az autó mentéséhez!",
    saveCarErrorNoUser: "Hiba: Nincs bejelentkezett felhasználó!",
    saveCarSuccess: "Autó sikeresen mentve!",
    saveCarError: "Hiba történt az autó mentésekor!",
    manufacturingStartYear: "Gyártás kezdete",
    manufacturingEndYear: "Gyártás vége",
    passengerCount: "Utaskapacitás",
    dimensions: "Méretek",
    weight: "Súly",
    transmission: "Sebességváltó",
    drivetrain: "Hajtás",
    vin: "VIN",
    address: "Cím",
    capacity: "Kapacitás",
    km: "Kilométerek",
    appointmentDate: "Időpont",
    branch: "Telephely",
    telephone: "Telefonszám",
    licenseplate: "Rendszám",
    repairHistory: "Javított kocsi:",
    yes: "Igen",
    no: "Nem",
    motExpiry: "Műszaki viszga lejárat",
    isSmoking: "Dohányzázott jármű",
  },
  jp: {
    searchTitle: "検索",
    brand: "ブランド名:",
    model: "モデル名:",
    modelCode: "型式",
    eg: "例:",
    bodyType: "ボディタイプ:",
    fuel: "燃料タイプ:",
    minPrice: "最低価格:",
    maxPrice: "最大価格:",
    yearFrom: "年式（開始）:",
    yearTo: "年式（終了）:",
    nameKanji: "",
    chooseLocation: "店舗を選択",
    moreSearch: "詳細検索",
    search: "検索",
    homepage: "ホーム",
    allCar: "すべての車",
    cars: "車を探す",
    services: "サービス",
    contact: "お問い合わせ",
    login: "ログイン",
    footer: "© 2025 車販売店。全著作権所有。",
    noResults: "検索条件に一致する結果はありません。",
    details: "詳細情報",
    price: "価格:",
    year: "年式:",
    type: "ボディタイプ:",
    fuelType: "燃料タイプ:",
    selectLocation: "店舗を選択してください",
    complete: "完了",
    username: "ユーザー名",
    password: "パスワード",
    register: "新規登録",
    switchToRegister: "新規登録へ切り替え",
    switchToLogin: "ログインへ切り替え",
    myProfile: "マイプロフィール",
    myMessages: "メッセージ",
    logout: "ログアウト",
    advertisement: "広告を投稿する",
    advertisementTitle: "車の広告を投稿",
    registerTitle: "新規登録",
    email: "メールアドレス:",
    registrationSuccess: "登録が成功しました！ログインしてください。",
    loginSuccess: "ログインに成功しました！",
    loginError: "ユーザー名またはパスワードが無効です。",
    footer_contact: "お問い合わせ",
    footer_company: "プレミアムカー株式会社 (Premium Cars Co., Ltd.)",
    footer_address: "〒100-0001 東京都千代田区千代田1-1",
    footer_tax: "税番号: 12345678-1-12",
    footer_follow_us: "フォローする",
    footer_facebook: "Facebook",
    footer_instagram: "Instagram",
    footer_links: "便利なリンク",
    footer_about: "会社概要",
    footer_contact_page: "お問い合わせ",
    footer_privacy: "プライバシーポリシー",
    footer_rights: "© 2025 プレミアムカー株式会社。全著作権所有。\n ここに記載されているすべての内容は、デモンストレーション目的でのみ使用されています。",
    view_cars: "車を見る",
    loginTitle: "ログイン",
    name: "名前",
    phoneNumber: "電話番号",
    successLogin: "ログイン成功！",
    successRegister: "登録成功！ログインしてください。",
    error: "入力情報が正しくありません。",
    postAd: "広告を投稿",
    locations: "店舗",
    description: "説明",
    mileage: "走行距離",
    color: "色",
    langu:"言語",
      successMessage: "報告の投稿成功しました。",
      uploadImage: "画像をアップロード",
      selectBrand: "銘柄",
      selectModel: "モデル",
      dealership: "中古車販売店",
      closed: "閉店",
      searchResult: "検索結果",
      welcomeText: "中古販売店へようこそ",
      searchCars: "私たちの自動車販売店のウェブサイトへようこそ！豊富な中古車のラインアップからお好きなものをお選びください！",
      viewCars: "車を見る",
      drive: "駆動",
      engineSize: "エンジンサイズ",
      mileageNum: "走行距離",
      savedCars: "保存されている車",
      noSavedCars: "保存されている車がありません。",
      savedCarsHome: "保存されている車",
      noMessage: "メッセージがありません",
      sender: "差出人",
      loginForData: "プロフィール拝見するためにログインしてください",
      myData: "情報",
      save: "保存",
      viewDetails: "詳細説明",
      chooseBrand: "ブランドを選択",
      noModel: "ブランドは存在しません",
      chooseModel: "モデルを選んで",
      chooseBodyType: "ボディタイプを選択",
      chooseFuel: "燃料を選択",
      chooseDrivetrain: "駆動を選択",
      chooseColor: "色を選択",
      hungarian: "ハンガリー語",
  english: "英語",
  japanese: "日本語",
    Year: "年式",
  Price: "値段",
    mass: "重さ",
    errorLoadingCar: "車のデータを読み込む際にエラーが発生しました",
    loginToSave: "この車を保存するにはログインしてください！",
    errorNoUser: "エラー：ログインしているユーザーがいません！",
    carSaved: "車が正常に保存されました！",
    errorSavingCar: "車を保存中にエラーが発生しました！",
    loginToBook: "予約するにはログインしてください！",
    chooseDate: "予約する日付を選択してください！",
    noPastDate: "過去の日付を選択することはできません！",
    alreadyBookedFirst: "すでに予約が入っています：",
    alreadyBookedSecond: "既存の予約をキャンセルする必要があります。",
    doneBookFirst: "予約が完了しました！",
    doneBookSecond: "ご来店をお待ちしております！",
    errorBooking: "予約処理中にエラーが発生しました。後でもう一度お試しください！",
    loading: "読み込み中...",
    back: "戻る",
    noImage: "画像なし",
    location: "店舗",
    noDetails: "詳細な説明はありません。",
    appointment: "予約する",
    mon: "月",
    tue: "火",
    wed: "水",
    thu: "木",
    fri: "金",
    sat: "土",
    sun: "日",
    time: "時間",
    cancel: "キャンセル",
    change: "変更",
    selectDate: "日付を選択",
    saveCarErrorNoAuth: "車を保存するにはログインしてください！",
    saveCarErrorNoUser: "エラー：ログインしているユーザーがいません！",
    saveCarSuccess: "車が正常に保存されました！",
    saveCarError: "車を保存中にエラーが発生しました！",

    manufacturingStartYear: "製造開始年",
    manufacturingEndYear: "製造終了年",
    passengerCount: "定員数",
    dimensions: "寸法",
    weight: "重量",
    transmission: "トランスミッション",
    drivetrain: "駆動方式",
    vin: "VIN",
    address: "住所",
    capacity: "最大定員",
    km: "走行距離",
    appointmentDate: "予約日",
    branch: "支店",
    telephone: "電話番号",
    licenseplate: "ナンバープレート",
    repairHistory: "修理履歴",
    yes: "はい",
    no: "いいえ",
    motExpiry: "車検の有効期限",
    isSmoking: "喫煙車",
  }
};
