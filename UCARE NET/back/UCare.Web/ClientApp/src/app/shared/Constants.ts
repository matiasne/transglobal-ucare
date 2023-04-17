export class Constants {

  public static get STATE_ACTIVE(): string { return "A" }
  public static get STATE_INACTIVE(): string { return "D" }
  public static get STATE_DELETED(): string { return "B" }

  public static get DELETE_YES(): string { return "1" }
  public static get DELETE_NO(): string { return "0" }

  public static get STRING_OK(): string { return "OK" }
  public static get STRING_ERROR(): string { return "error" }

  public static get LAYOUT_ACCESS_DENIED(): string { return "access-denied" }
  public static get LAYOUT_NOT_FOUNDD(): string { return "not-found" }
  public static get LAYOUT_ERROR(): string { return "error" }

  public static get LAYOUT_LOGIN(): string { return "login" }
  public static get LAYOUT_RECUPERAR(): string { return "recuperar" }
  
  public static get LAYOUT_HOME(): string { return "home" }
  public static get LAYOUT_USUARIOS(): string { return "usuarios" }
  public static get LAYOUT_TEMPO_SMS(): string { return "timepo-sms" }

  public static get LAYOUT_ROLES(): string { return "roles" }

  public static get LAYOUT_ESTADOS(): string { return "historial" }

  public static get LAYOUT_MAPA(): string { return "mapa" }

  public static get LAYOUT_MAPA_CONFIG(): string { return "mapa-config" }

  public static get LAYOUT_AFILIADO(): string { return "afiliado" }

  public static get LAYOUT_MONITOREO(): string { return "monitoreo" }

  public static get LAYOUT_COMUNICACION(): string { return "comunicacion" }

  public static PHONE_COUNTRY_CODES =
    [
      {
        "country": "Afghanistan",
        "countryCode": "93",
        "isoCode": "AF / AFG"
      },
      {
        "country": "Albania",
        "countryCode": "355",
        "isoCode": "AL / ALB"
      },
      {
        "country": "Algeria",
        "countryCode": "213",
        "isoCode": "DZ / DZA"
      },
      {
        "country": "American Samoa",
        "countryCode": "1-684",
        "isoCode": "AS / ASM"
      },
      {
        "country": "Andorra",
        "countryCode": "376",
        "isoCode": "AD / AND"
      },
      {
        "country": "Angola",
        "countryCode": "244",
        "isoCode": "AO / AGO"
      },
      {
        "country": "Anguilla",
        "countryCode": "1-264",
        "isoCode": "AI / AIA"
      },
      {
        "country": "Antarctica",
        "countryCode": "672",
        "isoCode": "AQ / ATA"
      },
      {
        "country": "Antigua and Barbuda",
        "countryCode": "1-268",
        "isoCode": "AG / ATG"
      },
      {
        "country": "Argentina",
        "countryCode": "54",
        "isoCode": "AR / ARG"
      },
      {
        "country": "Armenia",
        "countryCode": "374",
        "isoCode": "AM / ARM"
      },
      {
        "country": "Aruba",
        "countryCode": "297",
        "isoCode": "AW / ABW"
      },
      {
        "country": "Australia",
        "countryCode": "61",
        "isoCode": "AU / AUS"
      },
      {
        "country": "Austria",
        "countryCode": "43",
        "isoCode": "AT / AUT"
      },
      {
        "country": "Azerbaijan",
        "countryCode": "994",
        "isoCode": "AZ / AZE"
      },
      {
        "country": "Bahamas",
        "countryCode": "1-242",
        "isoCode": "BS / BHS"
      },
      {
        "country": "Bahrain",
        "countryCode": "973",
        "isoCode": "BH / BHR"
      },
      {
        "country": "Bangladesh",
        "countryCode": "880",
        "isoCode": "BD / BGD"
      },
      {
        "country": "Barbados",
        "countryCode": "1-246",
        "isoCode": "BB / BRB"
      },
      {
        "country": "Belarus",
        "countryCode": "375",
        "isoCode": "BY / BLR"
      },
      {
        "country": "Belgium",
        "countryCode": "32",
        "isoCode": "BE / BEL"
      },
      {
        "country": "Belize",
        "countryCode": "501",
        "isoCode": "BZ / BLZ"
      },
      {
        "country": "Benin",
        "countryCode": "229",
        "isoCode": "BJ / BEN"
      },
      {
        "country": "Bermuda",
        "countryCode": "1-441",
        "isoCode": "BM / BMU"
      },
      {
        "country": "Bhutan",
        "countryCode": "975",
        "isoCode": "BT / BTN"
      },
      {
        "country": "Bolivia",
        "countryCode": "591",
        "isoCode": "BO / BOL"
      },
      {
        "country": "Bosnia and Herzegovina",
        "countryCode": "387",
        "isoCode": "BA / BIH"
      },
      {
        "country": "Botswana",
        "countryCode": "267",
        "isoCode": "BW / BWA"
      },
      {
        "country": "Brazil",
        "countryCode": "55",
        "isoCode": "BR / BRA"
      },
      {
        "country": "British Indian Ocean Territory",
        "countryCode": "246",
        "isoCode": "IO / IOT"
      },
      {
        "country": "British Virgin Islands",
        "countryCode": "1-284",
        "isoCode": "VG / VGB"
      },
      {
        "country": "Brunei",
        "countryCode": "673",
        "isoCode": "BN / BRN"
      },
      {
        "country": "Bulgaria",
        "countryCode": "359",
        "isoCode": "BG / BGR"
      },
      {
        "country": "Burkina Faso",
        "countryCode": "226",
        "isoCode": "BF / BFA"
      },
      {
        "country": "Burundi",
        "countryCode": "257",
        "isoCode": "BI / BDI"
      },
      {
        "country": "Cambodia",
        "countryCode": "855",
        "isoCode": "KH / KHM"
      },
      {
        "country": "Cameroon",
        "countryCode": "237",
        "isoCode": "CM / CMR"
      },
      {
        "country": "Canada",
        "countryCode": "1",
        "isoCode": "CA / CAN"
      },
      {
        "country": "Cape Verde",
        "countryCode": "238",
        "isoCode": "CV / CPV"
      },
      {
        "country": "Cayman Islands",
        "countryCode": "1-345",
        "isoCode": "KY / CYM"
      },
      {
        "country": "Central African Republic",
        "countryCode": "236",
        "isoCode": "CF / CAF"
      },
      {
        "country": "Chad",
        "countryCode": "235",
        "isoCode": "TD / TCD"
      },
      {
        "country": "Chile",
        "countryCode": "56",
        "isoCode": "CL / CHL"
      },
      {
        "country": "China",
        "countryCode": "86",
        "isoCode": "CN / CHN"
      },
      {
        "country": "Christmas Island",
        "countryCode": "61",
        "isoCode": "CX / CXR"
      },
      {
        "country": "Cocos Islands",
        "countryCode": "61",
        "isoCode": "CC / CCK"
      },
      {
        "country": "Colombia",
        "countryCode": "57",
        "isoCode": "CO / COL"
      },
      {
        "country": "Comoros",
        "countryCode": "269",
        "isoCode": "KM / COM"
      },
      {
        "country": "Cook Islands",
        "countryCode": "682",
        "isoCode": "CK / COK"
      },
      {
        "country": "Costa Rica",
        "countryCode": "506",
        "isoCode": "CR / CRI"
      },
      {
        "country": "Croatia",
        "countryCode": "385",
        "isoCode": "HR / HRV"
      },
      {
        "country": "Cuba",
        "countryCode": "53",
        "isoCode": "CU / CUB"
      },
      {
        "country": "Curacao",
        "countryCode": "599",
        "isoCode": "CW / CUW"
      },
      {
        "country": "Cyprus",
        "countryCode": "357",
        "isoCode": "CY / CYP"
      },
      {
        "country": "Czech Republic",
        "countryCode": "420",
        "isoCode": "CZ / CZE"
      },
      {
        "country": "Democratic Republic of the Congo",
        "countryCode": "243",
        "isoCode": "CD / COD"
      },
      {
        "country": "Denmark",
        "countryCode": "45",
        "isoCode": "DK / DNK"
      },
      {
        "country": "Djibouti",
        "countryCode": "253",
        "isoCode": "DJ / DJI"
      },
      {
        "country": "Dominica",
        "countryCode": "1-767",
        "isoCode": "DM / DMA"
      },
      {
        "country": "Dominican Republic",
        "countryCode": "1-809, 1-829, 1-849",
        "isoCode": "DO / DOM"
      },
      {
        "country": "East Timor",
        "countryCode": "670",
        "isoCode": "TL / TLS"
      },
      {
        "country": "Ecuador",
        "countryCode": "593",
        "isoCode": "EC / ECU"
      },
      {
        "country": "Egypt",
        "countryCode": "20",
        "isoCode": "EG / EGY"
      },
      {
        "country": "El Salvador",
        "countryCode": "503",
        "isoCode": "SV / SLV"
      },
      {
        "country": "Equatorial Guinea",
        "countryCode": "240",
        "isoCode": "GQ / GNQ"
      },
      {
        "country": "Eritrea",
        "countryCode": "291",
        "isoCode": "ER / ERI"
      },
      {
        "country": "Estonia",
        "countryCode": "372",
        "isoCode": "EE / EST"
      },
      {
        "country": "Ethiopia",
        "countryCode": "251",
        "isoCode": "ET / ETH"
      },
      {
        "country": "Falkland Islands",
        "countryCode": "500",
        "isoCode": "FK / FLK"
      },
      {
        "country": "Faroe Islands",
        "countryCode": "298",
        "isoCode": "FO / FRO"
      },
      {
        "country": "Fiji",
        "countryCode": "679",
        "isoCode": "FJ / FJI"
      },
      {
        "country": "Finland",
        "countryCode": "358",
        "isoCode": "FI / FIN"
      },
      {
        "country": "France",
        "countryCode": "33",
        "isoCode": "FR / FRA"
      },
      {
        "country": "French Polynesia",
        "countryCode": "689",
        "isoCode": "PF / PYF"
      },
      {
        "country": "Gabon",
        "countryCode": "241",
        "isoCode": "GA / GAB"
      },
      {
        "country": "Gambia",
        "countryCode": "220",
        "isoCode": "GM / GMB"
      },
      {
        "country": "Georgia",
        "countryCode": "995",
        "isoCode": "GE / GEO"
      },
      {
        "country": "Germany",
        "countryCode": "49",
        "isoCode": "DE / DEU"
      },
      {
        "country": "Ghana",
        "countryCode": "233",
        "isoCode": "GH / GHA"
      },
      {
        "country": "Gibraltar",
        "countryCode": "350",
        "isoCode": "GI / GIB"
      },
      {
        "country": "Greece",
        "countryCode": "30",
        "isoCode": "GR / GRC"
      },
      {
        "country": "Greenland",
        "countryCode": "299",
        "isoCode": "GL / GRL"
      },
      {
        "country": "Grenada",
        "countryCode": "1-473",
        "isoCode": "GD / GRD"
      },
      {
        "country": "Guam",
        "countryCode": "1-671",
        "isoCode": "GU / GUM"
      },
      {
        "country": "Guatemala",
        "countryCode": "502",
        "isoCode": "GT / GTM"
      },
      {
        "country": "Guernsey",
        "countryCode": "44-1481",
        "isoCode": "GG / GGY"
      },
      {
        "country": "Guinea",
        "countryCode": "224",
        "isoCode": "GN / GIN"
      },
      {
        "country": "Guinea-Bissau",
        "countryCode": "245",
        "isoCode": "GW / GNB"
      },
      {
        "country": "Guyana",
        "countryCode": "592",
        "isoCode": "GY / GUY"
      },
      {
        "country": "Haiti",
        "countryCode": "509",
        "isoCode": "HT / HTI"
      },
      {
        "country": "Honduras",
        "countryCode": "504",
        "isoCode": "HN / HND"
      },
      {
        "country": "Hong Kong",
        "countryCode": "852",
        "isoCode": "HK / HKG"
      },
      {
        "country": "Hungary",
        "countryCode": "36",
        "isoCode": "HU / HUN"
      },
      {
        "country": "Iceland",
        "countryCode": "354",
        "isoCode": "IS / ISL"
      },
      {
        "country": "India",
        "countryCode": "91",
        "isoCode": "IN / IND"
      },
      {
        "country": "Indonesia",
        "countryCode": "62",
        "isoCode": "ID / IDN"
      },
      {
        "country": "Iran",
        "countryCode": "98",
        "isoCode": "IR / IRN"
      },
      {
        "country": "Iraq",
        "countryCode": "964",
        "isoCode": "IQ / IRQ"
      },
      {
        "country": "Ireland",
        "countryCode": "353",
        "isoCode": "IE / IRL"
      },
      {
        "country": "Isle of Man",
        "countryCode": "44-1624",
        "isoCode": "IM / IMN"
      },
      {
        "country": "Israel",
        "countryCode": "972",
        "isoCode": "IL / ISR"
      },
      {
        "country": "Italy",
        "countryCode": "39",
        "isoCode": "IT / ITA"
      },
      {
        "country": "Ivory Coast",
        "countryCode": "225",
        "isoCode": "CI / CIV"
      },
      {
        "country": "Jamaica",
        "countryCode": "1-876",
        "isoCode": "JM / JAM"
      },
      {
        "country": "Japan",
        "countryCode": "81",
        "isoCode": "JP / JPN"
      },
      {
        "country": "Jersey",
        "countryCode": "44-1534",
        "isoCode": "JE / JEY"
      },
      {
        "country": "Jordan",
        "countryCode": "962",
        "isoCode": "JO / JOR"
      },
      {
        "country": "Kazakhstan",
        "countryCode": "7",
        "isoCode": "KZ / KAZ"
      },
      {
        "country": "Kenya",
        "countryCode": "254",
        "isoCode": "KE / KEN"
      },
      {
        "country": "Kiribati",
        "countryCode": "686",
        "isoCode": "KI / KIR"
      },
      {
        "country": "Kosovo",
        "countryCode": "383",
        "isoCode": "XK / XKX"
      },
      {
        "country": "Kuwait",
        "countryCode": "965",
        "isoCode": "KW / KWT"
      },
      {
        "country": "Kyrgyzstan",
        "countryCode": "996",
        "isoCode": "KG / KGZ"
      },
      {
        "country": "Laos",
        "countryCode": "856",
        "isoCode": "LA / LAO"
      },
      {
        "country": "Latvia",
        "countryCode": "371",
        "isoCode": "LV / LVA"
      },
      {
        "country": "Lebanon",
        "countryCode": "961",
        "isoCode": "LB / LBN"
      },
      {
        "country": "Lesotho",
        "countryCode": "266",
        "isoCode": "LS / LSO"
      },
      {
        "country": "Liberia",
        "countryCode": "231",
        "isoCode": "LR / LBR"
      },
      {
        "country": "Libya",
        "countryCode": "218",
        "isoCode": "LY / LBY"
      },
      {
        "country": "Liechtenstein",
        "countryCode": "423",
        "isoCode": "LI / LIE"
      },
      {
        "country": "Lithuania",
        "countryCode": "370",
        "isoCode": "LT / LTU"
      },
      {
        "country": "Luxembourg",
        "countryCode": "352",
        "isoCode": "LU / LUX"
      },
      {
        "country": "Macau",
        "countryCode": "853",
        "isoCode": "MO / MAC"
      },
      {
        "country": "Macedonia",
        "countryCode": "389",
        "isoCode": "MK / MKD"
      },
      {
        "country": "Madagascar",
        "countryCode": "261",
        "isoCode": "MG / MDG"
      },
      {
        "country": "Malawi",
        "countryCode": "265",
        "isoCode": "MW / MWI"
      },
      {
        "country": "Malaysia",
        "countryCode": "60",
        "isoCode": "MY / MYS"
      },
      {
        "country": "Maldives",
        "countryCode": "960",
        "isoCode": "MV / MDV"
      },
      {
        "country": "Mali",
        "countryCode": "223",
        "isoCode": "ML / MLI"
      },
      {
        "country": "Malta",
        "countryCode": "356",
        "isoCode": "MT / MLT"
      },
      {
        "country": "Marshall Islands",
        "countryCode": "692",
        "isoCode": "MH / MHL"
      },
      {
        "country": "Mauritania",
        "countryCode": "222",
        "isoCode": "MR / MRT"
      },
      {
        "country": "Mauritius",
        "countryCode": "230",
        "isoCode": "MU / MUS"
      },
      {
        "country": "Mayotte",
        "countryCode": "262",
        "isoCode": "YT / MYT"
      },
      {
        "country": "Mexico",
        "countryCode": "52",
        "isoCode": "MX / MEX"
      },
      {
        "country": "Micronesia",
        "countryCode": "691",
        "isoCode": "FM / FSM"
      },
      {
        "country": "Moldova",
        "countryCode": "373",
        "isoCode": "MD / MDA"
      },
      {
        "country": "Monaco",
        "countryCode": "377",
        "isoCode": "MC / MCO"
      },
      {
        "country": "Mongolia",
        "countryCode": "976",
        "isoCode": "MN / MNG"
      },
      {
        "country": "Montenegro",
        "countryCode": "382",
        "isoCode": "ME / MNE"
      },
      {
        "country": "Montserrat",
        "countryCode": "1-664",
        "isoCode": "MS / MSR"
      },
      {
        "country": "Morocco",
        "countryCode": "212",
        "isoCode": "MA / MAR"
      },
      {
        "country": "Mozambique",
        "countryCode": "258",
        "isoCode": "MZ / MOZ"
      },
      {
        "country": "Myanmar",
        "countryCode": "95",
        "isoCode": "MM / MMR"
      },
      {
        "country": "Namibia",
        "countryCode": "264",
        "isoCode": "NA / NAM"
      },
      {
        "country": "Nauru",
        "countryCode": "674",
        "isoCode": "NR / NRU"
      },
      {
        "country": "Nepal",
        "countryCode": "977",
        "isoCode": "NP / NPL"
      },
      {
        "country": "Netherlands",
        "countryCode": "31",
        "isoCode": "NL / NLD"
      },
      {
        "country": "Netherlands Antilles",
        "countryCode": "599",
        "isoCode": "AN / ANT"
      },
      {
        "country": "New Caledonia",
        "countryCode": "687",
        "isoCode": "NC / NCL"
      },
      {
        "country": "New Zealand",
        "countryCode": "64",
        "isoCode": "NZ / NZL"
      },
      {
        "country": "Nicaragua",
        "countryCode": "505",
        "isoCode": "NI / NIC"
      },
      {
        "country": "Niger",
        "countryCode": "227",
        "isoCode": "NE / NER"
      },
      {
        "country": "Nigeria",
        "countryCode": "234",
        "isoCode": "NG / NGA"
      },
      {
        "country": "Niue",
        "countryCode": "683",
        "isoCode": "NU / NIU"
      },
      {
        "country": "North Korea",
        "countryCode": "850",
        "isoCode": "KP / PRK"
      },
      {
        "country": "Northern Mariana Islands",
        "countryCode": "1-670",
        "isoCode": "MP / MNP"
      },
      {
        "country": "Norway",
        "countryCode": "47",
        "isoCode": "NO / NOR"
      },
      {
        "country": "Oman",
        "countryCode": "968",
        "isoCode": "OM / OMN"
      },
      {
        "country": "Pakistan",
        "countryCode": "92",
        "isoCode": "PK / PAK"
      },
      {
        "country": "Palau",
        "countryCode": "680",
        "isoCode": "PW / PLW"
      },
      {
        "country": "Palestine",
        "countryCode": "970",
        "isoCode": "PS / PSE"
      },
      {
        "country": "Panama",
        "countryCode": "507",
        "isoCode": "PA / PAN"
      },
      {
        "country": "Papua New Guinea",
        "countryCode": "675",
        "isoCode": "PG / PNG"
      },
      {
        "country": "Paraguay",
        "countryCode": "595",
        "isoCode": "PY / PRY"
      },
      {
        "country": "Peru",
        "countryCode": "51",
        "isoCode": "PE / PER"
      },
      {
        "country": "Philippines",
        "countryCode": "63",
        "isoCode": "PH / PHL"
      },
      {
        "country": "Pitcairn",
        "countryCode": "64",
        "isoCode": "PN / PCN"
      },
      {
        "country": "Poland",
        "countryCode": "48",
        "isoCode": "PL / POL"
      },
      {
        "country": "Portugal",
        "countryCode": "351",
        "isoCode": "PT / PRT"
      },
      {
        "country": "Puerto Rico",
        "countryCode": "1-787, 1-939",
        "isoCode": "PR / PRI"
      },
      {
        "country": "Qatar",
        "countryCode": "974",
        "isoCode": "QA / QAT"
      },
      {
        "country": "Republic of the Congo",
        "countryCode": "242",
        "isoCode": "CG / COG"
      },
      {
        "country": "Reunion",
        "countryCode": "262",
        "isoCode": "RE / REU"
      },
      {
        "country": "Romania",
        "countryCode": "40",
        "isoCode": "RO / ROU"
      },
      {
        "country": "Russia",
        "countryCode": "7",
        "isoCode": "RU / RUS"
      },
      {
        "country": "Rwanda",
        "countryCode": "250",
        "isoCode": "RW / RWA"
      },
      {
        "country": "Saint Barthelemy",
        "countryCode": "590",
        "isoCode": "BL / BLM"
      },
      {
        "country": "Saint Helena",
        "countryCode": "290",
        "isoCode": "SH / SHN"
      },
      {
        "country": "Saint Kitts and Nevis",
        "countryCode": "1-869",
        "isoCode": "KN / KNA"
      },
      {
        "country": "Saint Lucia",
        "countryCode": "1-758",
        "isoCode": "LC / LCA"
      },
      {
        "country": "Saint Martin",
        "countryCode": "590",
        "isoCode": "MF / MAF"
      },
      {
        "country": "Saint Pierre and Miquelon",
        "countryCode": "508",
        "isoCode": "PM / SPM"
      },
      {
        "country": "Saint Vincent and the Grenadines",
        "countryCode": "1-784",
        "isoCode": "VC / VCT"
      },
      {
        "country": "Samoa",
        "countryCode": "685",
        "isoCode": "WS / WSM"
      },
      {
        "country": "San Marino",
        "countryCode": "378",
        "isoCode": "SM / SMR"
      },
      {
        "country": "Sao Tome and Principe",
        "countryCode": "239",
        "isoCode": "ST / STP"
      },
      {
        "country": "Saudi Arabia",
        "countryCode": "966",
        "isoCode": "SA / SAU"
      },
      {
        "country": "Senegal",
        "countryCode": "221",
        "isoCode": "SN / SEN"
      },
      {
        "country": "Serbia",
        "countryCode": "381",
        "isoCode": "RS / SRB"
      },
      {
        "country": "Seychelles",
        "countryCode": "248",
        "isoCode": "SC / SYC"
      },
      {
        "country": "Sierra Leone",
        "countryCode": "232",
        "isoCode": "SL / SLE"
      },
      {
        "country": "Singapore",
        "countryCode": "65",
        "isoCode": "SG / SGP"
      },
      {
        "country": "Sint Maarten",
        "countryCode": "1-721",
        "isoCode": "SX / SXM"
      },
      {
        "country": "Slovakia",
        "countryCode": "421",
        "isoCode": "SK / SVK"
      },
      {
        "country": "Slovenia",
        "countryCode": "386",
        "isoCode": "SI / SVN"
      },
      {
        "country": "Solomon Islands",
        "countryCode": "677",
        "isoCode": "SB / SLB"
      },
      {
        "country": "Somalia",
        "countryCode": "252",
        "isoCode": "SO / SOM"
      },
      {
        "country": "South Africa",
        "countryCode": "27",
        "isoCode": "ZA / ZAF"
      },
      {
        "country": "South Korea",
        "countryCode": "82",
        "isoCode": "KR / KOR"
      },
      {
        "country": "South Sudan",
        "countryCode": "211",
        "isoCode": "SS / SSD"
      },
      {
        "country": "Spain",
        "countryCode": "34",
        "isoCode": "ES / ESP"
      },
      {
        "country": "Sri Lanka",
        "countryCode": "94",
        "isoCode": "LK / LKA"
      },
      {
        "country": "Sudan",
        "countryCode": "249",
        "isoCode": "SD / SDN"
      },
      {
        "country": "Suriname",
        "countryCode": "597",
        "isoCode": "SR / SUR"
      },
      {
        "country": "Svalbard and Jan Mayen",
        "countryCode": "47",
        "isoCode": "SJ / SJM"
      },
      {
        "country": "Swaziland",
        "countryCode": "268",
        "isoCode": "SZ / SWZ"
      },
      {
        "country": "Sweden",
        "countryCode": "46",
        "isoCode": "SE / SWE"
      },
      {
        "country": "Switzerland",
        "countryCode": "41",
        "isoCode": "CH / CHE"
      },
      {
        "country": "Syria",
        "countryCode": "963",
        "isoCode": "SY / SYR"
      },
      {
        "country": "Taiwan",
        "countryCode": "886",
        "isoCode": "TW / TWN"
      },
      {
        "country": "Tajikistan",
        "countryCode": "992",
        "isoCode": "TJ / TJK"
      },
      {
        "country": "Tanzania",
        "countryCode": "255",
        "isoCode": "TZ / TZA"
      },
      {
        "country": "Thailand",
        "countryCode": "66",
        "isoCode": "TH / THA"
      },
      {
        "country": "Togo",
        "countryCode": "228",
        "isoCode": "TG / TGO"
      },
      {
        "country": "Tokelau",
        "countryCode": "690",
        "isoCode": "TK / TKL"
      },
      {
        "country": "Tonga",
        "countryCode": "676",
        "isoCode": "TO / TON"
      },
      {
        "country": "Trinidad and Tobago",
        "countryCode": "1-868",
        "isoCode": "TT / TTO"
      },
      {
        "country": "Tunisia",
        "countryCode": "216",
        "isoCode": "TN / TUN"
      },
      {
        "country": "Turkey",
        "countryCode": "90",
        "isoCode": "TR / TUR"
      },
      {
        "country": "Turkmenistan",
        "countryCode": "993",
        "isoCode": "TM / TKM"
      },
      {
        "country": "Turks and Caicos Islands",
        "countryCode": "1-649",
        "isoCode": "TC / TCA"
      },
      {
        "country": "Tuvalu",
        "countryCode": "688",
        "isoCode": "TV / TUV"
      },
      {
        "country": "U.S. Virgin Islands",
        "countryCode": "1-340",
        "isoCode": "VI / VIR"
      },
      {
        "country": "Uganda",
        "countryCode": "256",
        "isoCode": "UG / UGA"
      },
      {
        "country": "Ukraine",
        "countryCode": "380",
        "isoCode": "UA / UKR"
      },
      {
        "country": "United Arab Emirates",
        "countryCode": "971",
        "isoCode": "AE / ARE"
      },
      {
        "country": "United Kingdom",
        "countryCode": "44",
        "isoCode": "GB / GBR"
      },
      {
        "country": "United States",
        "countryCode": "1",
        "isoCode": "US / USA"
      },
      {
        "country": "Uruguay",
        "countryCode": "598",
        "isoCode": "UY / URY"
      },
      {
        "country": "Uzbekistan",
        "countryCode": "998",
        "isoCode": "UZ / UZB"
      },
      {
        "country": "Vanuatu",
        "countryCode": "678",
        "isoCode": "VU / VUT"
      },
      {
        "country": "Vatican",
        "countryCode": "379",
        "isoCode": "VA / VAT"
      },
      {
        "country": "Venezuela",
        "countryCode": "58",
        "isoCode": "VE / VEN"
      },
      {
        "country": "Vietnam",
        "countryCode": "84",
        "isoCode": "VN / VNM"
      },
      {
        "country": "Wallis and Futuna",
        "countryCode": "681",
        "isoCode": "WF / WLF"
      },
      {
        "country": "Western Sahara",
        "countryCode": "212",
        "isoCode": "EH / ESH"
      },
      {
        "country": "Yemen",
        "countryCode": "967",
        "isoCode": "YE / YEM"
      },
      {
        "country": "Zambia",
        "countryCode": "260",
        "isoCode": "ZM / ZMB"
      },
      {
        "country": "Zimbabwe",
        "countryCode": "263",
        "isoCode": "ZW / ZWE"
      }
    ];

}
