var empty= [];
var dataAcknowledgement = [
{"id": "01", "text" : "Not Required"},
{"id": "02", "text" : "Required"},
{"id": "03", "text" : "Exempted due to Religious Reason"}];

var dataAcknowledgementType = [
{"id": "01", "text" : "Vehicle"},
{"id": "02", "text" : "Plaque"}];

var dataExemptionResult = [
{"id": "01", "text" : "Approved"},
{"id": "02", "text" : "Not Approved"}];

var dataSalut = [
{"id": "01", "text" : "Mr"},
{"id": "02", "text" : "Ms"},
{"id": "03", "text" : "Mrs"},
{"id": "04", "text" : "Miss"},
{"id": "05", "text" : "Dr"},
{"id": "06", "text" : "Rev"},
{"id": "07", "text" : "Prof"},
{"id": "08", "text" : "Judge"},
{"id": "09", "text" : "Sister"},
{"id": "10", "text" : "Father"},
{"id": "11", "text" : "Dr Rev"},
{"id": "12", "text" : "Dr the Hon"},
{"id": "13", "text" : "Justice"},
{"id": "14", "text" : "Lt Colonel"},
{"id": "15", "text" : "Prof Sir"},
{"id": "16", "text" : "The Hon"},
{"id": "17", "text" : "The Hon Dr"},
{"id": "18", "text" : "The Hon Mr"},
{"id": "19", "text" : "The Hon Mr Justice"},
{"id": "20", "text" : "Ven"},
{"id": "21", "text" : "Rev Dr"},
{"id": "22", "text" : "Elder"},
{"id": "23", "text" : "The Revd"},
{"id": "24", "text" : "The Hon Sir"},
{"id": "25", "text" : "Pastor (Rev)"},
{"id": "26", "text" : "Abbot"},
{"id": "27", "text" : "Lt. -Colonel"}];

var dataAllocationType = [
{"id": "01", "text" : "Lump Sum"},
{"id": "02", "text" : "Non Lump Sum"},
{"id": "03", "text" : "Block Grant"},
{"id": "04", "text" : "Pilot / Experimental"},
{"id": "05", "text" : "Others"}];

var dataApplicationType = [
{"id": "01", "text" : "New"},
{"id": "02", "text" : "Supplementary"},
{"id": "03", "text" : "Reclassified"},
{"id": "04", "text" : "Grant Transferred"}];

var dataItems = [
{"id": "01", "text" : "AC"},
{"id": "02", "text" : "CS"},
{"id": "03", "text" : "EW"},
{"id": "04", "text" : "FE"},
{"id": "05", "text" : "FO"},
{"id": "06", "text" : "OS"},
{"id": "07", "text" : "RV"},
{"id": "08", "text" : "VO"},
{"id": "09", "text" : "VP"}];

var dataApprovalAuthority = [
{"id": "01", "text" : "Director of Social Welfare"},
{"id": "02", "text" : "Secretary for Finance Services and the Treasury"},
{"id": "03", "text" : "Finance Committee"},
{"id": "04", "text" : "AD(S)"}
];

var dataVehicleDetails = [
{"id": "01", "text" : "Heavy Goods Vehicle"},
{"id": "02", "text" : "Medium Goods Vehicle"},
{"id": "03", "text" : "Light Goods Vehicle"},
{"id": "04", "text" : "Public Light Bus"},
{"id": "05", "text" : "Private Bus"},
{"id": "06", "text" : "Private Car"},
{"id": "07", "text" : "Special Purpose Vehicle"},
{"id": "08", "text" : "Private Light Bus"},
{"id": "09", "text" : "Public Bus"},
{"id": "10", "text" : "Motor Cycle"},
{"id": "11", "text" : "Private Car Van"}];

var dataAddressDistrict = [
{"id": "01", "text" : "Central & Western"},
{"id": "02", "text" : "Eastern"},
{"id": "03", "text" : "Southern "},
{"id": "04", "text" : "Wan Chai"},
{"id": "05", "text" : "Kowloon City"},
{"id": "06", "text" : "Kwun Tong"},
{"id": "07", "text" : "Sham Shui Po"},
{"id": "08", "text" : "Yau Tsim Mong"},
{"id": "09", "text" : "Wong Tai Sin"},
{"id": "10", "text" : "Islands"},
{"id": "11", "text" : "Kwai Tsing"},
{"id": "12", "text" : "North"},
{"id": "13", "text" : "Sai Kung"},
{"id": "14", "text" : "Sha Tin"},
{"id": "15", "text" : "Tai Po"},
{"id": "16", "text" : "Tsuen Wan"},
{"id": "17", "text" : "Tuen Mun"},
{"id": "18", "text" : "Yuen Long"}];

var dataAddressRegion = [
{"id": "01", "text" : "Kowloon"},
{"id": "02", "text" : "Hong Kong Island"},
{"id": "03", "text" : "New Territorries"}];

var dataProgressReports = [
{"id": "01", "text" : "1st"},
{"id": "02", "text" : "2nd"},
{"id": "03", "text" : "3rd"},
{"id": "04", "text" : "4th"},
{"id": "05", "text" : "Final"}];

var dataVehicleEngineType = [
{"id": "01", "text" : "Diesel"},
{"id": "02", "text" : "Gasoline"},
{"id": "03", "text" : "LPG"}];

var dataGrantType = [
{"id": "01", "text" : "Major"},
{"id": "02", "text" : "Minor"},
{"id": "03", "text" : "Block"}];

var dataLFProjectOfficer = [
{"id": "01", "text" : "S(LF)1"},
{"id": "02", "text" : "S(LF)2"},
{"id": "03", "text" : "EO(LF)1"},
{"id": "04", "text" : "EO(LF)2"},
{"id": "11", "text" : "EO(LF)3"},
{"id": "05", "text" : "EOII(LF)1"},
{"id": "06", "text" : "EOII(LF)2"},
{"id": "07", "text" : "EOII(LF)3"},
{"id": "08", "text" : "EOII(LF)4"},
{"id": "09", "text" : "EOII(LF)5"},
{"id": "10", "text" : "EOII(LF)(MS)"},
{"id": "12", "text" : "EA(LF)SD2"}];

var dataYesNo = [
{"id": "01", "text" : "Yes"},
{"id": "02", "text" : "No"}];

var dataLFPurchased = [
{"id": "01", "text" : "Yes"},
{"id": "02", "text" : "No"}];

var dataLFRecognized = [
{"id": "01", "text" : "Yes"},
{"id": "02", "text" : "No"}];

var dataVehicleDetailsManufacturer = [
{"id": "01", "text" : "Honda"},
{"id": "02", "text" : "Toyota"},
{"id": "03", "text" : "Mercedes-Benz"},
{"id": "04", "text" : "Mitsubishi"},
{"id": "05", "text" : "Ford"},
{"id": "06", "text" : "Nissan"},
{"id": "07", "text" : "Mazda"},
{"id": "08", "text" : "Volkswagen"},
{"id": "09", "text" : "Bedford"},
{"id": "10", "text" : "Isuzu"},
{"id": "11", "text" : "Hino"}];

var dataModificationWorks = [
{"id": "01", "text" : "Tail-lift"}];

var dataNatureOfProjectMain = [
{"id": "01", "text" : "Works Mainly"},
{"id": "02", "text" : "F&E Mainly"},
{"id": "03", "text" : "Time-defined Project"},
{"id": "04", "text" : "Others"},
{"id": "05", "text" : "Works and F&E"}];

var dataNatureOfProjectSub = [
{"id": "01", "text" : "Purpose Built"},
{"id": "02", "text" : "Entrustment (to HD with premises procurement)"},
{"id": "03", "text" : "Entrustment (to HD without premises procurement)"},
{"id": "04", "text" : "Entrustment (to ArchSD)"},
{"id": "05", "text" : "Entrustment (to EMSD)"},
{"id": "06", "text" : "Slopes"},
{"id": "07", "text" : "In-situ Expansion"},
{"id": "08", "text" : "Fitting-out/ Renovation/ etc"},
{"id": "09", "text" : "Information Technology"},
{"id": "10", "text" : "Business Improvement Plan"},
{"id": "11", "text" : "Survey"},
{"id": "12", "text" : "Feasibility Study"},
{"id": "13", "text" : "Experimental Projects (Thematic Approach)"},
{"id": "14", "text" : "Experimental Projects (Non-Thematic Approach)"},
{"id": "15", "text" : "Conference/ Seminar"},
{"id": "16", "text" : "Course"},
{"id": "17", "text" : "Publications"},
{"id": "18", "text" : "Vehicle Overhauling"},
{"id": "19", "text" : "Purchase (Miscellaneous)"},
{"id": "20", "text" : "Purchase (Vehicle)"},
{"id": "21", "text" : "Purchase (Plants & Machinery)"},
{"id": "22", "text" : "Fund Injection"},
{"id": "23", "text" : "Loan"},
{"id": "24", "text" : "Tide-over Grant"},
{"id": "25", "text" : "Special One-off Grant"},
{"id": "26", "text" : "One-off Subsidy"},
{"id": "27", "text" : "Additional Resources for NGOs"}];

var dataNewPurchase = [
{"id": "01", "text" : "Yes"},
{"id": "02", "text" : "No"}];

var dataPackageName = [
{"id": "01", "text" : "BIP"},
{"id": "02", "text" : "PC Replacement 2001"},
{"id": "03", "text" : "IT for Elders 2001"},
{"id": "04", "text" : "IT for Disabled 2001"},
{"id": "05", "text" : "Modernisation Prog of ICYSC 03"},
{"id": "06", "text" : "RCHE"},
{"id": "07", "text" : "Dementia Units in Sub. RCHE"},
{"id": "08", "text" : "Prog for IYOP 1999"},
{"id": "09", "text" : "New Demented Units for Elderly"},
{"id": "10", "text" : "IEAP"},
{"id": "11", "text" : "Continuum of Care in RCHE"},
{"id": "12", "text" : "F&E - 14 New Ips in 14 CCCs"},
{"id": "13", "text" : "2nd Batch IEAP"},
{"id": "14", "text" : "Mod Prog of ICTSC - 2nd Batch"},
{"id": "15", "text" : "Extended Care Program in DAC"},
{"id": "16", "text" : "Works Extension Program in SW"},
{"id": "17", "text" : "New Dawn IEAP"},
{"id": "18", "text" : "Special One-off Grant"},
{"id": "19", "text" : "Conversion Exer - LTCS Stage 1"},
{"id": "20", "text" : "DNESP"},
{"id": "21", "text" : "PCs for Training Use 1999"},
{"id": "22", "text" : "Mod Prog of CYC - 1998"},
{"id": "23", "text" : "Conversion Exer - LTCS Stage 2"},
{"id": "24", "text" : "In-situ Expansion of IHCS Ord"},
{"id": "25", "text" : "Conversion Exer - LTCS (Major)"},
{"id": "26", "text" : "Mod Prog of ICYSC - Last Batch"},
{"id": "27", "text" : "Conversion Exer - LTCS Stage 3"},
{"id": "28", "text" : "Modernisation Package"},
{"id": "29", "text" : "PC Replacement 2009"}];

var dataPremisesNature = [
{"id": "01", "text" : "Public Housing Estates Premises"},
{"id": "02", "text" : "Private Development"},
{"id": "03", "text" : "FSI Premises"},
{"id": "04", "text" : "Self-owned Premises"},
{"id": "05", "text" : "Other Rented Premises"},
{"id": "06", "text" : "Various Premises"},
{"id": "07", "text" : "Others"},
{"id": "08", "text" : "Non Public Housing Estates Premises"}];

var dataPremisesTied = [
{"id": "Y", "text" : "Yes"},
{"id": "N", "text" : "No"}];

var dataProgrammeServices = [
  {
    "id":"101",
    "text":"Social Security"
  },
  {
    "id":"206",
    "text":"Offenders"
  },
  {
    "id":"207",
    "text":"Drug Dependent Persons Treatment and Rehabilitation Centre"
  },
  {
    "id":"301",
    "text":"School Social Work"
  },
  {
    "id":"305",
    "text":"Children's Centre"
  },
  {
    "id":"306",
    "text":"Children's Centre with S/R"
  },
  {
    "id":"307",
    "text":"Children's and Youth Centre"
  },
  {
    "id":"308",
    "text":"Children's and Youth Centre with S/R"
  },
  {
    "id":"309",
    "text":"Youth Centre"
  },
  {
    "id":"310",
    "text":"Youth Centre with S/R"
  },
  {
    "id":"311",
    "text":"Outreaching Social Work"
  },
  {
    "id":"311",
    "text":"Outreaching social work"
  },
  {
    "id":"312",
    "text":"Uniformed Organisations"
  },
  {
    "id":"313",
    "text":"Miscellaneous Group and Community Work Service"
  },
  {
    "id":"314",
    "text":"Young People"
  },
  {
    "id":"315",
    "text":"Integrated Team"
  },
  {
    "id":"316",
    "text":"Counselling Centres for Psychotropic Substance Abusers"
  },
  {
    "id":"402",
    "text":"Hostel for the Elderly"
  },
  {
    "id":"403",
    "text":"Social Centre for the Elderly"
  },
  {
    "id":"405",
    "text":"Day Care Centre for the Elderly"
  },
  {
    "id":"406",
    "text":"Multi - services Centre For the Elderly"
  },
  {
    "id":"407",
    "text":"Host/Home for the Elderly with Meal Places"
  },
  {
    "id":"408",
    "text":"C & A Home ( Subvented )"
  },
  {
    "id":"409",
    "text":"Combined Home"
  },
  {
    "id":"410",
    "text":"Old People's Home"
  },
  {
    "id":"411",
    "text":"Transit Shelter"
  },
  {
    "id":"412",
    "text":"Home Cum C & A Unit"
  },
  {
    "id":"414",
    "text":"Pool Bus"
  },
  {
    "id":"415",
    "text":"Outreaching Service for the Elderly at Risk"
  },
  {
    "id":"416",
    "text":"Elderly and Medical Services"
  },
  {
    "id":"417",
    "text":"Services for Ex - drug Abusers"
  },
  {
    "id":"418",
    "text":"Bought Place Scheme ( Private Homes)"
  },
  {
    "id":"419",
    "text":"C & A Home ( Helping Hand )"
  },
  {
    "id":"420",
    "text":"Holiday Centre for the Elderly"
  },
  {
    "id":"421",
    "text":"Supervisory Support for the Elderly Service"
  },
  {
    "id":"422",
    "text":"Carers' Support Centre"
  },
  {
    "id":"423",
    "text":"Home Help Service"
  },
  {
    "id":"424",
    "text":"Old People's Home on Model Cost"
  },
  {
    "id":"425",
    "text":"Contract Home"
  },
  {
    "id":"426",
    "text":"District Elderly Community Centre"
  },
  {
    "id":"427",
    "text":"District Elderly Community Centre Cum Day Care Unit"
  },
  {
    "id":"428",
    "text":"Integrated Home Care Services Team"
  },
  {
    "id":"429",
    "text":"Neighbourhood Elderly Centre"
  },
  {
    "id":"430",
    "text":"Nursing Home"
  },
  {
    "id":"501",
    "text":"Family Casework"
  },
  {
    "id":"503",
    "text":"Day Creche"
  },
  {
    "id":"505",
    "text":"Foster Care"
  },
  {
    "id":"506",
    "text":"Residential Home (Under 6)"
  },
  {
    "id":"507",
    "text":"Family Life Education"
  },
  {
    "id":"508",
    "text":"Clinical Psychology Service"
  },
  {
    "id":"510",
    "text":"Refuge for the Women"
  },
  {
    "id":"511",
    "text":"Day Nursery"
  },
  {
    "id":"512",
    "text":"Employment Service"
  },
  {
    "id":"513",
    "text":"Inter - country Casework/Pot - migration"
  },
  {
    "id":"515",
    "text":"Small Group Home"
  },
  {
    "id":"517",
    "text":"Services for Street Sleepers"
  },
  {
    "id":"518",
    "text":"Residential Home ( Over 6 )"
  },
  {
    "id":"519",
    "text":"Family and Child Welfare"
  },
  {
    "id":"520",
    "text":"Extended Hours - Child Care Service"
  },
  {
    "id":"521",
    "text":"Family Aide Service"
  },
  {
    "id":"522",
    "text":"Occasional Child Care Centre"
  },
  {
    "id":"523",
    "text":"Inter - country Adoption"
  },
  {
    "id":"524",
    "text":"Family Crisis Intervention Centre"
  },
  {
    "id":"525",
    "text":"Integrated Family Services Centre"
  },
  {
    "id":"526",
    "text":"Kindergarten Cum Child Care Centre"
  },
  {
    "id":"527",
    "text":"Mutual Help Child Care Centre"
  },
  {
    "id":"528",
    "text":"Suicide Crisis Intervention Centre"
  },
  {
    "id":"601",
    "text":"Day Activity Centre"
  },
  {
    "id":"603",
    "text":"Sheltered Workshops Cum Hostels/Halfwry Houses"
  },
  {
    "id":"608",
    "text":"Supported Employment"
  },
  {
    "id":"610",
    "text":"Day Activity Centre/Home"
  },
  {
    "id":"611",
    "text":"Sheltered Workshop"
  },
  {
    "id":"614",
    "text":"Head Office"
  },
  {
    "id":"616",
    "text":"Halfway House"
  },
  {
    "id":"617",
    "text":"Early Education Training Centre"
  },
  {
    "id":"618",
    "text":"Integrated Programme"
  },
  {
    "id":"619",
    "text":"Special Child Care Centre"
  },
  {
    "id":"620",
    "text":"Special Child Care Centre/Hostel"
  },
  {
    "id":"622",
    "text":"Blind - rehabilitation/Training/Communication Department"
  },
  {
    "id":"623",
    "text":"Deaf - counsclling/Sign Language Interpretation/Training"
  },
  {
    "id":"624",
    "text":"Deaf - ear Mode Production Audiological Speech Therapy"
  },
  {
    "id":"627",
    "text":"Social & Recreational Centre For the Disabled"
  },
  {
    "id":"629",
    "text":"Activity Centre for Discharged Mental Patient"
  },
  {
    "id":"631",
    "text":"Long Stay Care Home"
  },
  {
    "id":"632",
    "text":"Rehabilitation"
  },
  {
    "id":"633",
    "text":"Small Group Home for the mildly Mentally Handicapped"
  },
  {
    "id":"634",
    "text":"C & A Hostel for Severely Disabled Persons"
  },
  {
    "id":"635",
    "text":"Community Rehabilitation Network"
  },
  {
    "id":"636",
    "text":"C & A Hostel for Aged Blind"
  },
  {
    "id":"637",
    "text":"Home - based Training Programme"
  },
  {
    "id":"638",
    "text":"Hostel for Severely Mentally Handicapped"
  },
  {
    "id":"639",
    "text":"Hostel for Moderately Mentalty Handicapped"
  },
  {
    "id":"640",
    "text":"Autistic CHildren Programme - SCCC"
  },
  {
    "id":"641",
    "text":"Commercial-hired Transport Service for the Disabled"
  },
  {
    "id":"642",
    "text":"Hostel for Physically Handicapped"
  },
  {
    "id":"643",
    "text":"Occasional Child Care for Disabled Children"
  },
  {
    "id":"644",
    "text":"Parents Resource Centre"
  },
  {
    "id":"645",
    "text":"Supervisory Support for Rehabilitation Service"
  },
  {
    "id":"646",
    "text":"Supported Hostel and Housing"
  },
  {
    "id":"647",
    "text":"Training & Activity Centre for ex-mentally ill persons"
  },
  {
    "id":"648",
    "text":"Hostels & Home for the Disabled"
  },
  {
    "id":"649",
    "text":"Vocational Training for the Disabled"
  },
  {
    "id":"650",
    "text":"Special School"
  },
  {
    "id":"651",
    "text":"Community Rehabilitation Centre"
  },
  {
    "id":"652",
    "text":"Integrated Rehabilitation Services Centre"
  },
  {
    "id":"653",
    "text":"Integrated Vocational Rehabilitation Service Centre"
  },
  {
    "id":"654",
    "text":"Integrated Vocational Training Centre"
  },
  {
    "id":"655",
    "text":"Multi-service Centre for Hearing Impaired Persons"
  },
  {
    "id":"702",
    "text":"Community Centre"
  },
  {
    "id":"704",
    "text":"Neighbourhood Level Community Development Project"
  },
  {
    "id":"705",
    "text":"Community Development"
  },
  {
    "id":"706",
    "text":"Integrated Neighbourhood Project"
  },
  {
    "id":"801",
    "text":"Central Administration"
  },
  {
    "id":"806",
    "text":"Supporting Services"
  },
  {
    "id":"807",
    "text":"Y2K Projects"
  },
  {
    "id":"808",
    "text":"IT Development"
  },
  {
    "id":"809",
    "text":"Clientele Information System"
  },
  {
    "id":"810",
    "text":"Overseas Conference"
  },
  {
    "id":"901",
    "text":"Multi-objectives"
  },
  {
    "id":"902",
    "text":"Integrated Services Centre"
  },
  {
    "id":"XXX",
    "text":"DUMMY RECORD"
  }
];

var dataProjectDistrict = [
{"id": "01", "text" : "Multi-Districts"},
{"id": "02", "text" : "Eastern/ Wanchai"},
{"id": "03", "text" : "Central & Western/ Islands"},
{"id": "04", "text" : "Southern"},
{"id": "05", "text" : "Kwun Tong"},
{"id": "06", "text" : "Wong Tai Sin/ Sai Kung"},
{"id": "07", "text" : "Yau Tsim/ Mongkok"},
{"id": "08", "text" : "Sham Shui Po"},
{"id": "09", "text" : "Kowloon City"},
{"id": "10", "text" : "Yuen Long"},
{"id": "11", "text" : "Shatin"},
{"id": "12", "text" : "Tai Po/ North"},
{"id": "13", "text" : "Tsuen Wan/ Kwai Tsing"},
{"id": "14", "text" : "Tuen Mun"},
{"id": "15", "text" : "Kowloon City/ Yau Tsim Mong"},
{"id": "16", "text" : "Central/ West/ Southern & Island"}];

var dataProjectStatusMain = [
{"id": "01", "text" : "Application Processing"},
{"id": "02", "text" : "Application Successful"},
{"id": "03", "text" : "After Approval"},
{"id": "04", "text" : "Application Unsuccessful"},
{"id": "05", "text" : "File Closed"}];

var dataProjectStatusSub = [
{"id": "01", "text" : "Application acknowledged"},
{"id": "02", "text" : "Applicaton under consideration"},
{"id": "03", "text" : "Pending for futher information from applicant"},
{"id": "04", "text" : "Pending for vetting result from service branches/ sections in SWD"},
{"id": "05", "text" : "Pending for support from other government departments including vetting authorities"},
{"id": "06", "text" : "Pending for support from other authorities other than government departments"},
{"id": "07", "text" : "Pending for endorsement"},
{"id": "08", "text" : "Pending for Signed Approval"}];

var dataServiceProjectOfficer =[
  {
    "id":"01",
    "text":"AD(RM)"
  },
  {
    "id":"02",
    "text":"C(C)"
  },
  {
    "id":"03",
    "text":"C(DV)"
  },
  {
    "id":"04",
    "text":"C(E)1"
  },
  {
    "id":"05",
    "text":"C(E)2"
  },
  {
    "id":"06",
    "text":"C(FCW)1"
  },
  {
    "id":"07",
    "text":"C(FCW)2"
  },
  {
    "id":"08",
    "text":"DSWO(KT)"
  },
  {
    "id":"09",
    "text":"DSWO(SSP)"
  },
  {
    "id":"10",
    "text":"S(C)2"
  },
  {
    "id":"11",
    "text":"S(E)3"
  },
  {
    "id":"12",
    "text":"S(IST)2"
  },
  {
    "id":"13",
    "text":"S(IST)3"
  },
  {
    "id":"14",
    "text":"S(LORCHE)2"
  },
  {
    "id":"15",
    "text":"S(RM)3"
  },
  {
    "id":"16",
    "text":"S(RM)4"
  },
  {
    "id":"17",
    "text":"S(RM)5"
  },
  {
    "id":"18",
    "text":"S(RM)6"
  },
  {
    "id":"19",
    "text":"S(SL)3"
  },
  {
    "id":"20",
    "text":"S(Y)1"
  },
  {
    "id":"21",
    "text":"SS(CI)"
  },
  {
    "id":"22",
    "text":"SS(CW)1"
  },
  {
    "id":"23",
    "text":"SS(E)1"
  },
  {
    "id":"24",
    "text":"SS(E)2"
  },
  {
    "id":"25",
    "text":"SS(E)4"
  },
  {
    "id":"26",
    "text":"SS(RM)3"
  },
  {
    "id":"27",
    "text":"SS(Y)1"
  },
  {
    "id":"28",
    "text":"SS(Y)2"
  },
  {
    "id":"29",
    "text":"C(SDT)"
  },
  {
    "id":"30",
    "text":"S(CCC)"
  },
  {
    "id":"31",
    "text":"S(RM)1"
  },
  {
    "id":"32",
    "text":"EO(P)NT"
  },
  {
    "id":"33",
    "text":"S(F)1"
  },
  {
    "id":"34",
    "text":"S(IST)4"
  },
  {
    "id":"35",
    "text":"S(F)2"
  },
  {
    "id":"36",
    "text":"S(E)4"
  },
  {
    "id":"37",
    "text":"S(SL)4"
  },
  {
    "id":"38",
    "text":"SS(RM)1"
  },
  {
    "id":"39",
    "text":"S(RM)2"
  },
  {
    "id":"40",
    "text":"S(E)5"
  },
  {
    "id":"41",
    "text":"A(E)3"
  },
  {
    "id":"42",
    "text":"C(IST)"
  },
  {
    "id":"43",
    "text":"A(Y)2"
  },
  {
    "id":"44",
    "text":"S(C)1"
  },
  {
    "id":"45",
    "text":"SS(F)2"
  },
  {
    "id":"46",
    "text":"SS(RM)4"
  },
  {
    "id":"47",
    "text":"SS(SFS)"
  },
  {
    "id":"48",
    "text":"S(IST)8"
  },
  {
    "id":"49",
    "text":"EO(P)HK/G"
  },
  {
    "id":"50",
    "text":"SS(RM)5"
  },
  {
    "id":"51",
    "text":"SS(RM)2"
  },
  {
    "id":"52",
    "text":"ADSWO(YL)"
  },
  {
    "id":"53",
    "text":"DSWO(TM)"
  },
  {
    "id":"54",
    "text":"S(F)3"
  },
  {
    "id":"55",
    "text":"S(E)2"
  },
  {
    "id":"56",
    "text":"SS(E)5"
  },
  {
    "id":"57",
    "text":"S(RM)7"
  },
  {
    "id":"58",
    "text":"C(SS)4"
  },
  {
    "id":"59",
    "text":"S(S)1"
  },
  {
    "id":"60",
    "text":"A(E)1"
  },
  {
    "id":"61",
    "text":"SS(IST)2"
  },
  {
    "id":"62",
    "text":"S(Y)4"
  },
  {
    "id":"63",
    "text":"S(RM)9"
  },
  {
    "id":"64",
    "text":"S(SFS)3"
  },
  {
    "id":"65",
    "text":"ADSWO(SSP)2"
  },
  {
    "id":"66",
    "text":"A(Y)1"
  },
  {
    "id":"67",
    "text":"SWO(P/C)(CW/S/I)4"
  },
  {
    "id":"68",
    "text":"S(S)6"
  },
  {
    "id":"69",
    "text":"S(RM)8"
  },
  {
    "id":"70",
    "text":"A(Y)3"
  },
  {
    "id":"71",
    "text":"SS(E)3"
  },
  {
    "id":"72",
    "text":"S(S)4"
  },
  {
    "id":"73",
    "text":"A(CW)1"
  },
  {
    "id":"74",
    "text":"A(Y)5"
  },
  {
    "id":"75",
    "text":"A(E)5"
  },
  {
    "id":"76",
    "text":"S(E)1"
  },
  {
    "id":"77",
    "text":"S(DV)1"
  },
  {
    "id":"78",
    "text":"A(Y)6"
  },
  {
    "id":"79",
    "text":"SRM)13"
  },
  {
    "id":"80",
    "text":"S(RM)13"
  },
  {
    "id":"81",
    "text":"S(S)10"
  }
];

var dataCentreUnitServiceStatusAsAtProjectApplication = [
{"id": "01", "text" : "Existing"},
{"id": "02", "text" : "New"},
{"id": "03", "text" : "Reprovisioning"}];

var dataSubventedCurrently = [
{"id": "Y", "text" : "Yes"},
{"id": "N", "text" : "No"}];

var dataCentreUnitStatus = [
{"id": "01", "text" : "to be discussed"}];

var dataVehicleStatus = [
{"id": "01", "text" : "Serving"},
{"id": "02", "text" : "Scrapped (together with vehicle number)"},
{"id": "03", "text" : "Scrapped (vehicle number retained)"},
{"id": "04", "text" : "Transferred (together with vehicle number)"},
{"id": "05", "text" : "Transferred (vehicle number retained)"},
{"id": "06", "text" : "Replaced (with same vehicle number)"},
{"id": "07", "text" : "Replaced (with different vehicle number)"}];

var dataVehicleEntitlement = [
{"id": "01", "text" : "Yes"},
{"id": "02", "text" : "No"}];

var dataStageAP = [
{"id": "01", "text" : "01 - Initial Vetting"},
{"id": "02", "text" : "02 - Service & Technical Vetting"}];

var datSubStageAP = [
{"id": "01", "text" : "Application acknowledge"},
{"id": "02", "text" : "Complete set of documents received"}];

var datSubStageAP2 = [
{"id": "01", "text" : "No Objection from Branch"},
{"id": "02", "text" : "Comments on bidding documents to NGO"},
{"id": "03", "text" : "Permission for bidding"},
{"id": "04", "text" : "Bidding result to ArchSD"}];

var dataStageProcessing = [
{"id": "01", "text" : "01 - Initial Vetting"},
{"id": "02", "text" : "02 - Service Vetting"},
{"id": "03", "text" : "03 - Technical Vetting"},
{"id": "04", "text" : "04 - Project Approval"}];

var dataSubStageProcessing = [
{"id": "01", "text" : "Application acknowledged"},
{"id": "02", "text" : "Complet set of dicuments received"}];

var dataSubStageProcessing2 = [
{"id": "01", "text" : "Application forwarded to Branch"},
{"id": "02", "text" : "Application forwarded to EDB"},
{"id": "03", "text" : "Application forwarded to DSWO"},
{"id": "04", "text" : "Joint site visit"},
{"id": "05", "text" : "No objection from Licensing Office"},
{"id": "06", "text" : "Comments to NGO"},
{"id": "07", "text" : "No further comment on application"}];

var dataSubStageProcessing3 = [
{"id": "01", "text" : "Application forwarded to ArchSD"},
{"id": "02", "text" : "Applicaton forwarded to EMSD"},
{"id": "03", "text" : "Application forwarded to Arch Section"},
{"id": "04", "text" : "Comments to NGO"},
{"id": "05", "text" : "Cost estimation completed"}];

var dataSubStageMonitoring = [
{"id": "01", "text" : "LFAC meeting"},
{"id": "02", "text" : "Pending FSTB approval"},
{"id": "03", "text" : "Application approved"},
{"id": "04", "text" : "Approval letter issued"}];

var dataSubStageMonitoring2 = [
{"id": "01", "text" : "01 - Bidding of AP"},
{"id": "02", "text" : "02 - Tendering for Contractor"},
{"id": "03", "text" : "03 - Commencement of Works"},
{"id": "04", "text" : "04 - Project Completion"}];

var dataSubStageMonitoring3 = [
{"id": "01", "text" : "Draft bidding document to ArchSD"},
{"id": "02", "text" : "Draft bidding document to Arch Section"},
{"id": "03", "text" : "Comments to NGO"},
{"id": "04", "text" : "Permission for bidding"},
{"id": "05", "text" : "Bidding result to ArchSD"},
{"id": "06", "text" : "No objection for recommendation of AP"}];

var dataSubStageMonitoring4 = [
{"id": "01", "text" : "Submission of draft tender document for contractor"},
{"id": "02", "text" : "Permission for tendering"},
{"id": "03", "text" : "Tender analysis report to ArchSD"},
{"id": "04", "text" : "Permission for awarding tender"},
{"id": "05", "text" : "Approval for supplementary grant"},
{"id": "06", "text" : "Approval for awarding contract"}];

var dataSubStageMonitoring5 = [
{"id": "01", "text" : "Commencement of works"},
{"id": "02", "text" : "Request for VO to Branch"},
{"id": "03", "text" : "Request for VO to ArchSD"},
{"id": "04", "text" : "Approval-in-principle for VO to NGO"}];

var dataSubStageMonitoring6 = [
{"id": "01", "text" : "Final account forwarded to ArchSD"},
{"id": "02", "text" : "Final account forwarded to Arch Section"},
{"id": "03", "text" : "Recognised project cost reverted to NGO"},
{"id": "04", "text" : "Application for supplementary grant"},
{"id": "05", "text" : "Approval of supplementary grant"},
{"id": "06", "text" : "Final payment claim recognised"},
{"id": "07", "text" : "Project completion"},
{"id": "08", "text" : "Unspent balance reverted to fund"}];

var dataApplicationStatus = [
{"id": "01", "text" : "Applied"},
{"id": "02", "text" : "Not Applied"}];

var agencyData =
[
  {
    "agencyCode":"790",
    "subvented":"Yes",
    "agencyName":"SALVATION ARMY, THE"
  },
  {
    "agencyCode":"967",
    "subvented":"",
    "agencyName":"YOUTH OUTREACH"
  },
  {
    "agencyCode":"906",
    "subvented":"",
    "agencyName":"UNITED CHRISTIAN NETHERSOLE COMMUNITY HEALTH SERVICE"
  },
  {
    "agencyCode":"025",
    "subvented":"",
    "agencyName":"ALICE HO MIU LING NETHERSOLE CHARITY FOUNDATION"
  },
  {
    "agencyCode":"037",
    "subvented":"",
    "agencyName":"ASBURY METHODIST SOCIAL SERVICE"
  },
  {
    "agencyCode":"300",
    "subvented":"No",
    "agencyName":"HONG CHI ASSOCIATION"
  },
  {
    "agencyCode":"844",
    "subvented":"Yes",
    "agencyName":"FU HONG SOCIETY"
  },
  {
    "agencyCode":"974",
    "subvented":"Yes",
    "agencyName":"YUEN YUEN INSTITUTE, THE"
  },
  {
    "agencyCode":"476",
    "subvented":"No",
    "agencyName":"HONG KONG SOCIETY FOR REHABILITATION, THE"
  },
  {
    "agencyCode":"929",
    "subvented":"Yes",
    "agencyName":"WOMEN'S WELFARE CLUB, WESTERN DISTRICT, HONG KONG"
  },
  {
    "agencyCode":"268",
    "subvented":"Yes",
    "agencyName":"HAVEN OF HOPE CHRISTIAN SERVICE"
  },
  {
    "agencyCode":"837",
    "subvented":"",
    "agencyName":"SOCIAL WELFARE DEPARTMENT"
  },
  {
    "agencyCode":"340",
    "subvented":"No",
    "agencyName":"HONG KONG CHRISTIAN SERVICE"
  },
  {
    "agencyCode":"811",
    "subvented":"No",
    "agencyName":"SELF HELP GROUP FOR THE BRAIN DAMAGED"
  },
  {
    "agencyCode":"120",
    "subvented":"Yes",
    "agencyName":"CARITAS - HONG KONG"
  },
  {
    "agencyCode":"218",
    "subvented":"Yes",
    "agencyName":"ELCHK, SOCIAL SERVICE HEAD OFFICE"
  },
  {
    "agencyCode":"730",
    "subvented":"Yes",
    "agencyName":"PO LEUNG KUK"
  },
  {
    "agencyCode":"896",
    "subvented":"Yes",
    "agencyName":"TUNG WAH GROUP OF HOSPITALS"
  },
  {
    "agencyCode":"078",
    "subvented":"Yes",
    "agencyName":"BOYS' AND GIRLS' CLUBS ASSOCIATION OF HONG KONG, THE"
  }
];

var applicationData =
[
  {
    "serialNo":6635,
    "fileRef":"LFPS/124/66(1)",
    "grantType":"Major",
    "applicationType":"New",
    "dateOfApplication":"27/09/2012",
    "projectDescription":"Replacing the existing 7-seater petrol private car with a new 14-seater LPG private light bus for its Kwun Tong Integrated Home Care Service Team.",
    "projectStatus":"Application Successful",
    "agency":"SALVATION ARMY, THE",
    "centreName":"Kwun Tong Integrated Home Care Services Team"
  },
  {
    "serialNo":6434,
    "fileRef":"LFPS/209/198(1)",
    "grantType":"Major",
    "applicationType":"New",
    "dateOfApplication":"03/05/2012",
    "projectDescription":"Furnishing and equipping a new Day Activity Centre cum Hostel for Severely Mentally Handicapped Persons in Tin Ching Estate, Tin Shui Wai",
    "projectStatus":"Application Successful",
    "agency":"SOCIAL WELFARE DEPARTMENT",
    "centreName":"Day Activity Centre cum Hostel for Severely Mentally Handicapped Persons in Tin Shui Wai"
  },
  {
    "serialNo":6668,
    "fileRef":"LFPS/219/198",
    "grantType":"Major",
    "applicationType":"New",
    "dateOfApplication":"23/04/2013",
    "projectDescription":"Fitting-out works for setting up a new Neighbourhood Elderly Centre in Kowloon City.",
    "projectStatus":"Application Successful",
    "agency":"SOCIAL WELFARE DEPARTMENT",
    "centreName":"Neighbourhood Elderly Centre in Kowloon City"
  },
  {
    "serialNo":6802,
    "fileRef":"BG 364(12)",
    "grantType":"Block",
    "applicationType":"Supplementary",
    "dateOfApplication":"09/07/2013",
    "projectDescription":"",
    "projectStatus":"Application Successful",
    "agency":"YOUTH OUTREACH",
    "centreName":""
  },
  {
    "serialNo":6803,
    "fileRef":"BG 375(12)",
    "grantType":"Block",
    "applicationType":"Supplementary",
    "dateOfApplication":"09/07/2013",
    "projectDescription":"",
    "projectStatus":"Application Successful",
    "agency":"UNITED CHRISTIAN NETHERSOLE COMMUNITY HEALTH SERVICE",
    "centreName":""
  },
  {
    "serialNo":6804,
    "fileRef":"BG 409(12)",
    "grantType":"Block",
    "applicationType":"Supplementary",
    "dateOfApplication":"09/07/2013",
    "projectDescription":"",
    "projectStatus":"Application Successful",
    "agency":"ALICE HO MIU LING NETHERSOLE CHARITY FOUNDATION",
    "centreName":""
  },
  {
    "serialNo":6805,
    "fileRef":"BG 429(12)",
    "grantType":"Block",
    "applicationType":"Supplementary",
    "dateOfApplication":"09/07/2013",
    "projectDescription":"",
    "projectStatus":"Application Successful",
    "agency":"ASBURY METHODIST SOCIAL SERVICE",
    "centreName":""
  },
  {
    "serialNo":6806,
    "fileRef":"BG 48(12)",
    "grantType":"Block",
    "applicationType":"Supplementary",
    "dateOfApplication":"09/07/2013",
    "projectDescription":"",
    "projectStatus":"Application Successful",
    "agency":"CARITAS - HONG KONG",
    "centreName":""
  },
  {
    "serialNo":6807,
    "fileRef":"LFPS/37/82(2)",
    "grantType":"Major",
    "applicationType":"New",
    "dateOfApplication":"21/03/2013",
    "projectDescription":"Purchasing vehicle for its Hong Chi Morninghope School",
    "projectStatus":"Application Unsuccessful",
    "agency":"HONG CHI ASSOCIATION",
    "centreName":"Hong Chi Morninghope School, Wu King"
  },
  {
    "serialNo":6808,
    "fileRef":"LFPS/3/176(23)",
    "grantType":"Minor",
    "applicationType":"New",
    "dateOfApplication":"30/05/2013",
    "projectDescription":"Purchasing furniture and equipment for its Shun Lee Adult Training Centre",
    "projectStatus":"Application Successful",
    "agency":"FU HONG SOCIETY",
    "centreName":"(Replaced) Shun Lee Adult Training Centre (by 5045)"
  },
  {
    "serialNo":6809,
    "fileRef":"NP 5/72(1)",
    "grantType":"Major",
    "applicationType":"New",
    "dateOfApplication":"11/10/2011",
    "projectDescription":"Replenishing furniture and equipment for its Yuen Yuen Nursing Home (Sau Mau Ping Estate) in Kwun Tong",
    "projectStatus":"Application Successful",
    "agency":"YUEN YUEN INSTITUTE, THE",
    "centreName":"Yuen Yuen Nursing Home"
  },
  {
    "serialNo":6814,
    "fileRef":"LFPS/15/21(5)",
    "grantType":"Major",
    "applicationType":"Supplementary",
    "dateOfApplication":"23/11/2009",
    "projectDescription":"Carrying out slope Upgrading and Associated Drainage Repairing Works for the Hong Kong Society for Rehabilitation Lam Tin Complex",
    "projectStatus":"Application Successful",
    "agency":"HONG KONG SOCIETY FOR REHABILITATION, THE",
    "centreName":"LAM TIN COMPLEX"
  },
  {
    "serialNo":6815,
    "fileRef":"LFPS/134/66",
    "grantType":"Major",
    "applicationType":"Supplementary",
    "dateOfApplication":"18/07/2013",
    "projectDescription":"Replacing the existing 14-seater diesel private light bus with a new 14-seater LPG private light bus for its Kam Tin Residence for Senior Citizens.",
    "projectStatus":"Application Successful",
    "agency":"SALVATION ARMY, THE",
    "centreName":"Kam Tin Residence for Senior Citizens"
  },
  {
    "serialNo":6816,
    "fileRef":"LFPS/4/180(3)",
    "grantType":"Minor",
    "applicationType":"New",
    "dateOfApplication":"05/06/2013",
    "projectDescription":"Replacing the fire service installations for its David Woo Memorial Day Nursery in Shau Kei Wan",
    "projectStatus":"Application Successful",
    "agency":"WOMEN'S WELFARE CLUB, WESTERN DISTRICT, HONG KONG",
    "centreName":"DAVID WOO MEMORIAL DN"
  },
  {
    "serialNo":6817,
    "fileRef":"LFPS/124/66(1)",
    "grantType":"Major",
    "applicationType":"Supplementary",
    "dateOfApplication":"18/07/2013",
    "projectDescription":"Replacing the existing 7-seater petrol private car with a new 14-seater LPG private light bus for its Kwun Tong Integrated Home Care Service Team.",
    "projectStatus":"Application Successful",
    "agency":"SALVATION ARMY, THE",
    "centreName":"Kwun Tong Integrated Home Care Services Team"
  },
  {
    "serialNo":6818,
    "fileRef":"NP 20/19(17)",
    "grantType":"Major",
    "applicationType":"Supplementary",
    "dateOfApplication":"25/06/2013",
    "projectDescription":"Renovating Tung Wah Group of Hospitals Hui Lai Kuen Home for the Elderly for Covnersion Programme",
    "projectStatus":"Application Successful",
    "agency":"TUNG WAH GROUP OF HOSPITALS",
    "centreName":"HUI LAI KUEN HOME FOR THE ELDERLY"
  },
  {
    "serialNo":6819,
    "fileRef":"LFPS/16/258(4)",
    "grantType":"Minor",
    "applicationType":"New",
    "dateOfApplication":"10/05/2013",
    "projectDescription":"Purchasing furniture and equipment for its Haven of Hope Nursing Home.",
    "projectStatus":"Application Successful",
    "agency":"HAVEN OF HOPE CHRISTIAN SERVICE",
    "centreName":"HAVEN OF HOPE NURSING HOME"
  },
  {
    "serialNo":6888,
    "fileRef":"LFPS/125/198",
    "grantType":"Major",
    "applicationType":"Supplementary",
    "dateOfApplication":"01/11/2013",
    "projectDescription":"Carrying out a Technical Feasibility Study for the redevelopment of ex-Kai Nang Sheltered Workshop and Hostel into an IRSC.",
    "projectStatus":"Application Successful",
    "agency":"SOCIAL WELFARE DEPARTMENT",
    "centreName":"Kai Nang Sheltered Workshop and Hostel"
  },
  {
    "serialNo":6889,
    "fileRef":"LFPS/147/33(1)",
    "grantType":"Minor",
    "applicationType":"New",
    "dateOfApplication":"24/09/2013",
    "projectDescription":"Purchasing furniture and equipment for the reprovisioning of its Fung Pak Lim Nursery in Sham Shui Po",
    "projectStatus":"Application Successful",
    "agency":"PO LEUNG KUK",
    "centreName":"FUNG PAK LIM NURSERY"
  },
  {
    "serialNo":6890,
    "fileRef":"LFPS/59/63(13)",
    "grantType":"Minor",
    "applicationType":"New",
    "dateOfApplication":"03/07/2013",
    "projectDescription":"Minor works and purchasing furniture and equipment for its Times Nursery School in Wan Chai",
    "projectStatus":"Application Successful",
    "agency":"HONG KONG CHRISTIAN SERVICE",
    "centreName":"Times Nursery School"
  },
  {
    "serialNo":6891,
    "fileRef":"LFPS/460(3)",
    "grantType":"Minor",
    "applicationType":"New",
    "dateOfApplication":"08/10/2013",
    "projectDescription":"Purchasing furniture and equipment for the reprovisioning of its Activity Centre in Sham Shui Po.",
    "projectStatus":"Application Successful",
    "agency":"SELF HELP GROUP FOR THE BRAIN DAMAGED",
    "centreName":"Activity Centre at Pak Tin Estate"
  },
  {
    "serialNo":6892,
    "fileRef":"LFPS/99/48(8)",
    "grantType":"Minor",
    "applicationType":"New",
    "dateOfApplication":"09/07/2013",
    "projectDescription":"Replenishing furniture and equipment for its Caritas Harold H.W. Lee Care and Attention Home in Shatin",
    "projectStatus":"Application Successful",
    "agency":"CARITAS - HONG KONG",
    "centreName":"HAROLD H.W. LEE C&amp;A HOME"
  },
  {
    "serialNo":6893,
    "fileRef":"LFPS/460(2)",
    "grantType":"Major",
    "applicationType":"New",
    "dateOfApplication":"8/10/2013",
    "projectDescription":"Fitting-out works for the reprovisioning of its Activity Centre in Sham Shui Po",
    "projectStatus":"Application Successful",
    "agency":"SELF HELP GROUP FOR THE BRAIN DAMAGED",
    "centreName":"Activity Centre at Pak Tin Estate"
  },
  {
    "serialNo":6894,
    "fileRef":"LFPS/5/214(23)",
    "grantType":"Minor",
    "applicationType":"New",
    "dateOfApplication":"24/06/2013",
    "projectDescription":"Purchasing furniture and equipment for ELCHK Shan King Care & Attention Home for the Elderly",
    "projectStatus":"Application Successful",
    "agency":"ELCHK, SOCIAL SERVICE HEAD OFFICE",
    "centreName":"SHAN KING CARE &amp; ATTENTION HOME FOR THE ELDERLY"
  },
  {
    "serialNo":6895,
    "fileRef":"LFPS/147/33",
    "grantType":"Major",
    "applicationType":"New",
    "dateOfApplication":"24/09/2013",
    "projectDescription":"Fitting-out works for reprovisioning - Po Leung Kuk Fung Pak Lim Nursery in Sham Shui Po",
    "projectStatus":"Application Successful",
    "agency":"PO LEUNG KUK",
    "centreName":"FUNG PAK LIM NURSERY"
  },
  {
    "serialNo":6896,
    "fileRef":"LFPS/148/33",
    "grantType":"Major",
    "applicationType":"Grant Transferred",
    "dateOfApplication":"03/10/2013",
    "projectDescription":"Fitting-out works for setting up a new Neighbourhood Elderly Centre in Kowloon City.",
    "projectStatus":"Application Successful",
    "agency":"PO LEUNG KUK",
    "centreName":"Neighbourhood Elderly Centre in Kowloon City"
  },
  {
    "serialNo":6897,
    "fileRef":"LFPS/146/33",
    "grantType":"Major",
    "applicationType":"Grant Transferred",
    "dateOfApplication":"29/08/2013",
    "projectDescription":"Fitting out works of a new Integrated Rehabilitation services centre in Shek Kip Mei Estate, Sham Shui Po.",
    "projectStatus":"Application Successful",
    "agency":"PO LEUNG KUK",
    "centreName":"Integrated Rehabilitation services centre in Sham Shui Po"
  },
  {
    "serialNo":6898,
    "fileRef":"LFPS/146/33(1)",
    "grantType":"Major",
    "applicationType":"Grant Transferred",
    "dateOfApplication":"29/08/2013",
    "projectDescription":"Furnishing and equipping a new Integrated Rehabilitation services centre in Shek Kip Mei Estate, Sham Shui Po.",
    "projectStatus":"Application Successful",
    "agency":"PO LEUNG KUK",
    "centreName":"Integrated Rehabilitation services centre in Sham Shui Po"
  },
  {
    "serialNo":6899,
    "fileRef":"LFPS/138/19",
    "grantType":"Major",
    "applicationType":"Grant Transferred",
    "dateOfApplication":"21/08/2013",
    "projectDescription":"Fitting out works of a new Day Activity Centre cum Hostel for Severely Mentally Handicapped Persons in Tin Ching Estate, Tin Shui Wai",
    "projectStatus":"Application Successful",
    "agency":"TUNG WAH GROUP OF HOSPITALS",
    "centreName":"Day Activity Centre cum Hostel for Severely Mentally Handicapped Persons in Tin Shui Wai"
  },
  {
    "serialNo":6900,
    "fileRef":"LFPS/138/19(1)",
    "grantType":"Major",
    "applicationType":"Grant Transferred",
    "dateOfApplication":"12/09/2013",
    "projectDescription":"Furnishing and equipping a new Day Activity Centre cum Hostel for Severely Mentally Handicapped Persons in Tin Ching Estate, Tin Shui Wai",
    "projectStatus":"Application Successful",
    "agency":"TUNG WAH GROUP OF HOSPITALS",
    "centreName":"Day Activity Centre cum Hostel for Severely Mentally Handicapped Persons in Tin Shui Wai"
  },
  {
    "serialNo":6901,
    "fileRef":"LFPS/95/18",
    "grantType":"Minor",
    "applicationType":"New",
    "dateOfApplication":"07/11/2013",
    "projectDescription":"Implementing a nine-month youth career development project for the progamme workers.",
    "projectStatus":"Application Successful",
    "agency":"BOYS' AND GIRLS' CLUBS ASSOCIATION OF HONG KONG, THE",
    "centreName":"Project on Career Development for Programme Workers"
  }
];

var centreUnitData =
[
  {
    "centreUnitCode":"L0955",
    "centreUnitName":"Day Activity Centre cum Hostel for Severely Mentally Handicapped Persons in Tin Shui Wai",
    "agencyName":"SOCIAL WELFARE DEPARTMENT",
    "premisesTied":"Yes",
    "subvented":"Yes"
  },
  {
    "centreUnitCode":"L0958",
    "centreUnitName":"Neighbourhood Elderly Centre in Kowloon City",
    "agencyName":"SOCIAL WELFARE DEPARTMENT",
    "premisesTied":"Yes",
    "subvented":"Yes"
  },
  {
    "centreUnitCode":"L0363",
    "centreUnitName":"Hong Chi Morninghope School, Wu King",
    "agencyName":"HONG CHI ASSOCIATION",
    "premisesTied":"No",
    "subvented":"No"
  },
  {
    "centreUnitCode":"L0478",
    "centreUnitName":"(Replaced) Shun Lee Adult Training Centre (by 5045)",
    "agencyName":"FU HONG SOCIETY",
    "premisesTied":"Yes",
    "subvented":"Yes"
  },
  {
    "centreUnitCode":"L0642",
    "centreUnitName":"Yuen Yuen Nursing Home",
    "agencyName":"YUEN YUEN INSTITUTE, THE",
    "premisesTied":"Yes",
    "subvented":"Yes"
  },
  {
    "centreUnitCode":"L0364",
    "centreUnitName":"LAM TIN COMPLEX",
    "agencyName":"HONG KONG SOCIETY FOR REHABILITATION, THE",
    "premisesTied":"No",
    "subvented":"No"
  },
  {
    "centreUnitCode":"L0862",
    "centreUnitName":"Kam Tin Residence for Senior Citizens",
    "agencyName":"SALVATION ARMY, THE",
    "premisesTied":"Yes",
    "subvented":"Yes"
  },
  {
    "centreUnitCode":"5538",
    "centreUnitName":"DAVID WOO MEMORIAL DN",
    "agencyName":"WOMEN'S WELFARE CLUB, WESTERN DISTRICT, HONG KONG",
    "premisesTied":"Yes",
    "subvented":"Yes"
  },
  {
    "centreUnitCode":"L0797",
    "centreUnitName":"Kwun Tong Integrated Home Care Services Team",
    "agencyName":"SALVATION ARMY, THE",
    "premisesTied":"Yes",
    "subvented":"Yes"
  },
  {
    "centreUnitCode":"5396",
    "centreUnitName":"HUI LAI KUEN HOME FOR THE ELDERLY",
    "agencyName":"TUNG WAH GROUP OF HOSPITALS",
    "premisesTied":"Yes",
    "subvented":"Yes"
  },
  {
    "centreUnitCode":"3830",
    "centreUnitName":"HAVEN OF HOPE NURSING HOME",
    "agencyName":"HAVEN OF HOPE CHRISTIAN SERVICE",
    "premisesTied":"Yes",
    "subvented":"Yes"
  },
  {
    "centreUnitCode":"L0952",
    "centreUnitName":"Kai Nang Sheltered Workshop and Hostel",
    "agencyName":"SOCIAL WELFARE DEPARTMENT",
    "premisesTied":"",
    "subvented":""
  },
  {
    "centreUnitCode":"4330",
    "centreUnitName":"FUNG PAK LIM NURSERY",
    "agencyName":"PO LEUNG KUK",
    "premisesTied":"Yes",
    "subvented":"Yes"
  },
  {
    "centreUnitCode":"L0536",
    "centreUnitName":"Times Nursery School",
    "agencyName":"HONG KONG CHRISTIAN SERVICE",
    "premisesTied":"No",
    "subvented":"No"
  },
  {
    "centreUnitCode":"L0493",
    "centreUnitName":"Activity Centre at Pak Tin Estate",
    "agencyName":"SELF HELP GROUP FOR THE BRAIN DAMAGED",
    "premisesTied":"No",
    "subvented":"No"
  },
  {
    "centreUnitCode":"1673",
    "centreUnitName":"HAROLD H.W. LEE C&amp;A HOME",
    "agencyName":"CARITAS - HONG KONG",
    "premisesTied":"Yes",
    "subvented":"Yes"
  },
  {
    "centreUnitCode":"L0493",
    "centreUnitName":"Activity Centre at Pak Tin Estate",
    "agencyName":"SELF HELP GROUP FOR THE BRAIN DAMAGED",
    "premisesTied":"No",
    "subvented":"No"
  },
  {
    "centreUnitCode":"2329",
    "centreUnitName":"SHAN KING CARE &amp; ATTENTION HOME FOR THE ELDERLY",
    "agencyName":"ELCHK, SOCIAL SERVICE HEAD OFFICE",
    "premisesTied":"Yes",
    "subvented":"Yes"
  },
  {
    "centreUnitCode":"L1001",
    "centreUnitName":"Neighbourhood Elderly Centre in Kowloon City",
    "agencyName":"PO LEUNG KUK",
    "premisesTied":"Yes",
    "subvented":"Yes"
  },
  {
    "centreUnitCode":"L1003",
    "centreUnitName":"Integrated Rehabilitation services centre in Sham Shui Po",
    "agencyName":"PO LEUNG KUK",
    "premisesTied":"Yes",
    "subvented":"Yes"
  },
  {
    "centreUnitCode":"L1004",
    "centreUnitName":"Day Activity Centre cum Hostel for Severely Mentally Handicapped Persons in Tin Shui Wai",
    "agencyName":"TUNG WAH GROUP OF HOSPITALS",
    "premisesTied":"Yes",
    "subvented":"Yes"
  },
  {
    "centreUnitCode":"L1005",
    "centreUnitName":"Project on Career Development for Programme Workers",
    "agencyName":"BOYS' AND GIRLS' CLUBS ASSOCIATION OF HONG KONG, THE",
    "premisesTied":"Yes",
    "subvented":"Yes"
  }
];

var vehicleData =
[
  {
    "vehicleMark":"JC5929",
    "agencyName":"SALVATION ARMY, THE",
    "centreUnitName":"Kwun Tong Integrated Home Care Services Team"
  },
  {
    "vehicleMark":"KD9560",
    "agencyName":"SALVATION ARMY, THE",
    "centreUnitName":"Kam Tin Residence for Senior Citizens"
  }
];
var userData =
[
  {"userName":"Chan Tai Man","post":"APMLF1","email":"apmlf1@test.com","group":"Administrator","groupCode":"01","sysAdmin":"Yes", "sysAdminInd":"01"},
  {"userName":"Chan Siu Man","post":"APMLF2","email":"apmlf2@test.com","group":"Supervisor","groupCode":"02","sysAdmin":"Yes", "sysAdminInd":"01"},
  {"userName":"Chan Man Tai","post":"APMLF3","email":"apmlf3@test.com","group":"Responsible Officer","groupCode":"03","sysAdmin":"No", "sysAdminInd":"02"},
  {"userName":"Chan Man Siu","post":"APMLF4","email":"apmlf4@test.com","group":"GR & Other Project Officers","groupCode":"04","sysAdmin":"No", "sysAdminInd":"02"}
];
var parametersData =
[
  {"code":"PAR001","description":"BU Alert - ? day(s) after an Application being drafted","value":"1"},
  {"code":"PAR002","description":"Sample Parameter","value":"Free Text allowed"}
];
var importExpenseData =
[
  {"fileName":"MAJOR1202.csv","noOfRecord":"1255","uploadDate":"25/02/2003","importDate":"07/02/2003"},
  {"fileName":"MINOR1202.csv","noOfRecord":"890","uploadDate":"26/02/2003","importDate":""}
];
var userLogReportData = [
  {"userName":"apmlf","function":"Login","action":"Login","actionTime":"31/12/2008 14:05:51","remark":"Success"},
  {"userName":"apmlf2","function":"Login","action":"Login","actionTime":"31/12/2008 14:25:03","remark":"Success"},
  {"userName":"apmlf2","function":"Report05","action":"Soft Output","actionTime":"31/12/2008 17:05:51","remark":"Success"},
  {"userName":"apmlf","function":"Report05","action":"Soft Output","actionTime":"31/12/2008 17:01:51","remark":"Success"},
];
var buData = [
  {"buDate":"12/12/2013","fileRef":"LFPS/124/66(1)","agencyName":"SALVATION ARMY, THE","centreUnitName":"Kwun Tong Integrated Home Care Services Team","projectDescription":"Replacing the existing 7-seater petrol private car with a new 14-seater LPG private light bus for its Kwun Tong Integrated Home Care Service Team.","remark":"Review the Grant XXXXX XXXXX XXXXX XXXXX XXXXX XXXXX XXXXX XXXXX...","href":"master-application.html","recipient":"apmlf1","recordId":"10000","recipient":"apmlf1,apmlf2"}
];
var userGroupData = [
  {"id":"01","text":"Administrator"},
  {"id":"02","text":"Supervisor"},
  {"id":"03","text":"Responsible Officer"},
  {"id":"04","text":"GR & Other Project Officers"},
  {"id":"05","text":"Public"}
];
var fileUploadData = [
  {"id":"1","fileName":"Test Doc.doc","uploadBy":"Admin","uploadDate":"01/01/2000 14:25:30"},
  {"id":"2","fileName":"Test Jpge.jpge","uploadBy":"Admin","uploadDate":"01/01/2000 15:25:30"},
  {"id":"3","fileName":"Test Pdf.pdf","uploadBy":"Admin","uploadDate":"01/01/2000 16:25:30"},
];
var messageData =
[
  {"code":"SYS001","description":"Message when a record is created","message":"Record Created Successfully"},
  {"code":"SYS002","description":"Message when a record is deleted","message":"Record Deleted Successfully"},
  {"code":"SYS003","description":"Message when a record is updated","message":"Record Updated Successfully"},
  {"code":"ERR003","description":"Message for a required field","message":"{0} is a required field"}
];
var dataVehicleMaker =
[
  {
    "id":"01",
    "text":"Honda"
  },
  {
    "id":"02",
    "text":"Toyota"
  },
  {
    "id":"03",
    "text":"Mercedes-Benz"
  },
  {
    "id":"04",
    "text":"Mitsubishi"
  },
  {
    "id":"05",
    "text":"Ford"
  },
  {
    "id":"06",
    "text":"Nissan"
  },
  {
    "id":"07",
    "text":"Mazda"
  },
  {
    "id":"08",
    "text":"Volkswagen"
  },
  {
    "id":"09",
    "text":"Bedford"
  },
  {
    "id":"10",
    "text":"Isuzu"
  },
  {
    "id":"11",
    "text":"Hino"
  }
];

var dataPost =
[
  {
    "id":"01",
    "text":"Accountant"
  },
  {
    "id":"02",
    "text":"Acting Assistant Chief Executive"
  },
  {
    "id":"03",
    "text":"Acting Chief Executive Officer"
  },
  {
    "id":"04",
    "text":"Administration Executive"
  },
  {
    "id":"05",
    "text":"Administration Executive Adviser"
  },
  {
    "id":"06",
    "text":"Administrative Assistant"
  },
  {
    "id":"07",
    "text":"Administrative Executive"
  },
  {
    "id":"08",
    "text":"Administrative Officer"
  },
  {
    "id":"09",
    "text":"Administrator"
  },
  {
    "id":"10",
    "text":"Agency Director"
  },
  {
    "id":"100",
    "text":"Superintendent (Precious Blood Children's Village)"
  },
  {
    "id":"101",
    "text":"Administrative Manager"
  },
  {
    "id":"102",
    "text":"Chairman of Social Service Division"
  },
  {
    "id":"103",
    "text":"Executive Secretary (Social Service Division)"
  },
  {
    "id":"104",
    "text":"Chief Executive, Elderly Service"
  },
  {
    "id":"105",
    "text":"Senior Manager (Financial Management)"
  },
  {
    "id":"106",
    "text":"General Manager"
  },
  {
    "id":"107",
    "text":"Chairman, Management Committee"
  },
  {
    "id":"108",
    "text":"Chairman of Executive Committee"
  },
  {
    "id":"109",
    "text":"Secretary of Executive Committee"
  },
  {
    "id":"11",
    "text":"Agency Secretary"
  },
  {
    "id":"110",
    "text":"Chairman of Social Committee"
  },
  {
    "id":"111",
    "text":"Chairman of Board"
  },
  {
    "id":"112",
    "text":"Chairman (HA Care)"
  },
  {
    "id":"113",
    "text":"Director, Hong Kong"
  },
  {
    "id":"114",
    "text":"Officer-in-charge"
  },
  {
    "id":"115",
    "text":"Chairman of Day Nursery Management Committee"
  },
  {
    "id":"116",
    "text":"Chairman of Board of Management"
  },
  {
    "id":"117",
    "text":"Pastor (Rev)"
  },
  {
    "id":"118",
    "text":"Board Chairman"
  },
  {
    "id":"119",
    "text":"Chief Executive Officer (Operations and Control)"
  },
  {
    "id":"12",
    "text":"Assistant Chief Executive"
  },
  {
    "id":"120",
    "text":"Honorary General Secretary"
  },
  {
    "id":"121",
    "text":"Director of Architectual Services"
  },
  {
    "id":"122",
    "text":"Chief Technical Adviser"
  },
  {
    "id":"123",
    "text":"Director of Electrical & Mechanical Services"
  },
  {
    "id":"124",
    "text":"BSE/GES/Central Support"
  },
  {
    "id":"125",
    "text":"Director of Housing"
  },
  {
    "id":"126",
    "text":"Service Manager"
  },
  {
    "id":"127",
    "text":"Senior Manager"
  },
  {
    "id":"128",
    "text":"Project Officer"
  },
  {
    "id":"129",
    "text":"Executive Director (Honorary)"
  },
  {
    "id":"13",
    "text":"Assistant Director"
  },
  {
    "id":"130",
    "text":"SOCIAL SERVICE OFFICER"
  },
  {
    "id":"131",
    "text":"Chairman, Executive Committee"
  },
  {
    "id":"132",
    "text":"Chairman of Social Welfare Service Committee"
  },
  {
    "id":"133",
    "text":"Pastor"
  },
  {
    "id":"134",
    "text":"Social Service Coordinator"
  },
  {
    "id":"135",
    "text":"Social Services Director"
  },
  {
    "id":"136",
    "text":"Chief Executive Director"
  },
  {
    "id":"137",
    "text":"Agency Head (Precious Blood Children's Village)"
  },
  {
    "id":"138",
    "text":"Church Clerk"
  },
  {
    "id":"139",
    "text":"Chief Supervisor"
  },
  {
    "id":"14",
    "text":"Assistant General Secretary"
  },
  {
    "id":"140",
    "text":"Planning & Coordinating Officer"
  },
  {
    "id":"15",
    "text":"Brigade Secretary"
  },
  {
    "id":"16",
    "text":"Central Administration Officer"
  },
  {
    "id":"17",
    "text":"Centre Administrator"
  },
  {
    "id":"18",
    "text":"Centre Director"
  },
  {
    "id":"19",
    "text":"Centre Supervisor"
  },
  {
    "id":"20",
    "text":"Centre-in-charge"
  },
  {
    "id":"21",
    "text":"Chairman"
  },
  {
    "id":"22",
    "text":"Chairman of Council"
  },
  {
    "id":"23",
    "text":"Chairman, Organising Committee"
  },
  {
    "id":"24",
    "text":"Chief Administrator"
  },
  {
    "id":"25",
    "text":"Chief Executive"
  },
  {
    "id":"26",
    "text":"Chief Executive Officer"
  },
  {
    "id":"27",
    "text":"Chief Scout Executive"
  },
  {
    "id":"28",
    "text":"Chief Secretary"
  },
  {
    "id":"29",
    "text":"Community Services Secretary"
  },
  {
    "id":"30",
    "text":"Co-ordinator"
  },
  {
    "id":"31",
    "text":"Deputy Chairman"
  },
  {
    "id":"32",
    "text":"Deputy Chief Executive"
  },
  {
    "id":"33",
    "text":"Deputy Executive Director"
  },
  {
    "id":"34",
    "text":"Director"
  },
  {
    "id":"35",
    "text":"Director (Social Service Division)"
  },
  {
    "id":"36",
    "text":"Director (Social Service)"
  },
  {
    "id":"37",
    "text":"Director for Social Services Dept"
  },
  {
    "id":"38",
    "text":"Director of Administration"
  },
  {
    "id":"39",
    "text":"Director of Social Work Services"
  },
  {
    "id":"40",
    "text":"Executive Director"
  },
  {
    "id":"41",
    "text":"Executive Officer"
  },
  {
    "id":"42",
    "text":"Executive Organizer"
  },
  {
    "id":"43",
    "text":"Executive Secretary"
  },
  {
    "id":"44",
    "text":"General Manager (Nursing)"
  },
  {
    "id":"45",
    "text":"General Secretary"
  },
  {
    "id":"46",
    "text":"General Secretary for Social Services"
  },
  {
    "id":"47",
    "text":"Head of Department of Social Work"
  },
  {
    "id":"48",
    "text":"Managing Director"
  },
  {
    "id":"49",
    "text":"Office Secretary"
  },
  {
    "id":"50",
    "text":"President"
  },
  {
    "id":"51",
    "text":"President (Chief of Service)"
  },
  {
    "id":"52",
    "text":"Program Secretary"
  },
  {
    "id":"53",
    "text":"Programme Co-ordinator"
  },
  {
    "id":"54",
    "text":"Programme Officer"
  },
  {
    "id":"55",
    "text":"Project Chairman"
  },
  {
    "id":"56",
    "text":"Regional Coordinator"
  },
  {
    "id":"57",
    "text":"School Principal"
  },
  {
    "id":"58",
    "text":"Secretary"
  },
  {
    "id":"59",
    "text":"Secretary General"
  },
  {
    "id":"60",
    "text":"Senior Social Service Manager"
  },
  {
    "id":"61",
    "text":"Service Co-ordinator"
  },
  {
    "id":"62",
    "text":"Service Supervisor"
  },
  {
    "id":"63",
    "text":"Social Services Co-ordinator"
  },
  {
    "id":"64",
    "text":"Social Services Secretary"
  },
  {
    "id":"65",
    "text":"Society Administrator"
  },
  {
    "id":"66",
    "text":"Superintendent"
  },
  {
    "id":"67",
    "text":"Supervisor"
  },
  {
    "id":"68",
    "text":"Vice President"
  },
  {
    "id":"69",
    "text":"Vice-chairlady"
  },
  {
    "id":"70",
    "text":"Welfare Worker"
  },
  {
    "id":"71",
    "text":"Chairlady"
  },
  {
    "id":"72",
    "text":"Chairperson"
  },
  {
    "id":"73",
    "text":"Co-chairman"
  },
  {
    "id":"74",
    "text":"Officer Commanding"
  },
  {
    "id":"75",
    "text":"Principal"
  },
  {
    "id":"76",
    "text":"Trustees"
  },
  {
    "id":"77",
    "text":"Vice Chairman"
  },
  {
    "id":"78",
    "text":"Administrator Manager"
  },
  {
    "id":"79",
    "text":"Acting Executive Secretary"
  },
  {
    "id":"80",
    "text":"Centre Manager"
  },
  {
    "id":"81",
    "text":"Chief Executive - Social Service"
  },
  {
    "id":"82",
    "text":"Executive Administrator"
  },
  {
    "id":"83",
    "text":"External President"
  },
  {
    "id":"84",
    "text":"Honorary Secretary"
  },
  {
    "id":"85",
    "text":"Service Development Officer"
  },
  {
    "id":"86",
    "text":"Services Coordinator"
  },
  {
    "id":"87",
    "text":"Social Service Executive"
  },
  {
    "id":"88",
    "text":"Supervisor (Precious Blood Children's Village)"
  },
  {
    "id":"89",
    "text":"Vice-President"
  },
  {
    "id":"90",
    "text":"Chief Staff Officer"
  },
  {
    "id":"91",
    "text":"Executive"
  },
  {
    "id":"92",
    "text":"Supervisor of Social Services"
  },
  {
    "id":"93",
    "text":"Chief Commissioner"
  },
  {
    "id":"94",
    "text":"Principal Social Service Secretary"
  },
  {
    "id":"95",
    "text":"The President"
  },
  {
    "id":"96",
    "text":"General Manager (Social Services)"
  },
  {
    "id":"97",
    "text":"Treasurer"
  },
  {
    "id":"98",
    "text":"Chairman of Synod"
  },
  {
    "id":"99",
    "text":"Chief Officer of Welfare Department"
  }
];
var dataBlockGrantDetailItem = [
{"id": "01", "text" : "F&E"},
{"id": "02", "text" : "Vehicle"},
{"id": "03", "text" : "Works"}];

var dataDocLibFolders = [
{"id": "dataDocLibRoot", "text" : "Root"},
{"id": "dataDocLibEngagementOfAP", "text" : "– Engagement of AP"},
{"id": "dataDocLibSelfFinancing", "text" : "– Self-financing"},
{"id": "dataDocLibNamingAfter", "text" : "– Naming After"}];
var dataDocLibRoot = [
  {"id":"1","fileName":"000-List of Documents.doc","uploadBy":"Admin","uploadDate":"01/01/2000 14:25:30","loc":"000-List of Documents.doc"},
  {"id":"2","fileName":"D105C-Lotteries Fund Manual (Chinese)_Jan.2013.pdf","uploadBy":"Admin","uploadDate":"01/01/2000 15:25:30","loc":"D105C-Lotteries Fund Manual (Chinese)_Jan.2013.pdf"},
  {"id":"3","fileName":"D105E-Lotteries Fund Manual (English)_Jan.2013.pdf","uploadBy":"Admin","uploadDate":"01/01/2000 16:25:30","loc":"D105E-Lotteries Fund Manual (English)_Jan.2013.pdf"}];
var dataDocLibEngagementOfAP = [];
var dataDocLibNamingAfter = [
  {"id":"1","fileName":"D705-Naming of LF project after a donor_dated 15.9.2009.pdf","uploadBy":"Admin","uploadDate":"01/01/2000 14:25:30","loc":"Naming After/D705-Naming of LF project after a donor_dated 15.9.2009.pdf"},
  {"id":"2","fileName":"D710-Naming of LF project after a donor_dated 2.11.2009.pdf","uploadBy":"Admin","uploadDate":"01/01/2000 15:25:30","loc":"Naming After/D710-Naming of LF project after a donor_dated 2.11.2009.pdf"},
  {"id":"3","fileName":"D715-Naming of LF project after a donor_dated 20.3.2013.pdf","uploadBy":"Admin","uploadDate":"01/01/2000 16:25:30","loc":"Naming After/D715-Naming of LF project after a donor_dated 20.3.2013.pdf"},
  {"id":"4","fileName":"D720AN-Naming of LF project after a donor_dated 20.3.2013.pdf","uploadBy":"Admin","uploadDate":"01/01/2000 16:25:30","loc":"Naming After/D720AN-Naming of LF project after a donor_dated 20.3.2013.pdf"},
  {"id":"5","fileName":"D720AP-Naming of LF project after a donor_dated 20.3.2013.pdf","uploadBy":"Admin","uploadDate":"01/01/2000 16:25:30","loc":"Naming After/D720AP-Naming of LF project after a donor_dated 20.3.2013.pdf"},
  {"id":"6","fileName":"D720-Naming of LF project after a donor_dated 20.3.2013.pdf","uploadBy":"Admin","uploadDate":"01/01/2000 16:25:30","loc":"Naming After/D720-Naming of LF project after a donor_dated 20.3.2013.pdf"}];
var dataDocLibSelfFinancing = [];
var dataOutstandingGrant =
[
  {
    "serialNo":12000,
    "fileRef":"LFPS/124/66(1)",
    "grantType":"Major",
    "applicationType":"New",
    "dateOfApplication":"27/09/2012",
    "projectDescription":"Replacing the existing 7-seater petrol private car with a new 14-seater LPG private light bus for its Kwun Tong Integrated Home Care Service Team.",
    "projectStatus":"Approved",
    "subProjectStatus":"Final Payment Claim completed",
    "agency":"SALVATION ARMY, THE",
    "centreName":"Kwun Tong Integrated Home Care Services Team"
  },
  {
    "serialNo":12001,
    "fileRef":"LFPS/999/66(1)",
    "grantType":"Major",
    "applicationType":"New",
    "dateOfApplication":"27/09/2012",
    "projectDescription":"Replacing the existing 5-seater petrol private car with a new 7-seater LPG private light bus for its Kwun Tong Integrated Home Care Service Team.",
    "projectStatus":"Application Processing",
    "subProjectStatus":"Application acknowledged",
    "agency":"SALVATION ARMY, THE",
    "centreName":"Kwun Tong Integrated Home Care Services Team"
  },
  {
    "serialNo":12002,
    "fileRef":"LFPS/996/198(1)",
    "grantType":"Major",
    "applicationType":"New",
    "dateOfApplication":"03/05/2012",
    "projectDescription":"Furnishing and equipping a new Day Activity Centre cum Hostel for Severely Mentally Handicapped Persons in Tin Ching Estate, Tin Shui Wai",
    "projectStatus":"After approved",
    "subProjectStatus":"Works Practically Completed",
    "agency":"SOCIAL WELFARE DEPARTMENT",
    "centreName":"Day Activity Centre cum Hostel for Severely Mentally Handicapped Persons in Tin Shui Wai"
  }
];
var dataPurposeBuildMailestones = [
  {"id":"01","text":"Feasibility Study"},
  {"id":"02","text":"Site Investigation"},
  {"id":"03","text":"Demolition"},
  {"id":"04","text":"Foundation"},
  {"id":"05","text":"Site Formation"},
  {"id":"06","text":"Superstructure"},
  {"id":"07","text":"Building Service Installations"},
  {"id":"08","text":"External Work & Landscape"}
];
var dataLetters =[
{'id': 'L01', 'text' : 'Approval Letter for Pilot Scheme (Home Care Services for Severe Disabilities)','enable':'01'},
{'id': 'L02', 'text' : 'Approval Letter for Pilot Scheme (Home Care Services for Frail Elders)','enable':'01'},
{'id': 'L03', 'text' : 'Approval Letter for Evaluation Study on Cyber Youth Projects','enable':'01'},
{'id': 'L04', 'text' : 'Approval Letter for HKU Project (Community-based Intervention to Suicide Clusters)','enable':'01'},
{'id': 'L05', 'text' : 'Approval Letter for HKU Project (Review of IT Strategy for Social Welfare Sector)','enable':'01'},
{'id': 'L06', 'text' : 'Approval Letter for F&E','enable':'01'},
{'id': 'L07', 'text' : 'Approval Letter for Fitting-out Works','enable':'01'},
{'id': 'L08', 'text' : 'Invitation Letter for Block Grant Application','enable':'01'},
{'id': 'L09', 'text' : 'Acknowledgement Letter for Application of Block Grant','enable':'01'},
{'id': 'L10', 'text' : 'Approval Letter for Block Grant','enable':'01'},
{'id': 'L11', 'text' : 'Reminder to NGOs for Submission of AFS and DU for Block Grant','enable':'01'},
{'id': 'L12', 'text' : 'Pre-AP Acknowledgement Letter (Supplementary Information Not Required)','enable':'01'},
{'id': 'L13', 'text' : 'Pre-AP Acknowledgement Letter (Supplementary Information Required)','enable':'01'},
{'id': 'L14', 'text' : 'Vetting of Pre-AP (For ArchSD Vetting)','enable':'01'},
{'id': 'L15', 'text' : 'Vetting of Pre-AP (For Service Branch Comment)','enable':'01'},
{'id': 'L16', 'text' : 'Invite Bidding of Pre-AP','enable':'01'},
{'id': 'L17', 'text' : 'Awarding Pre-AP','enable':'01'},
{'id': 'L18', 'text' : 'Acknowledgement Letter for Major Grant','enable':'01'},
{'id': 'L19', 'text' : 'Acknowledgement Letter for Minor Grant / KG-cum-CCC (English)','enable':'01'},
{'id': 'L20', 'text' : 'Acknowledgement Letter for Minor Grant / KG-cum-CCC (Chinese)','enable':'01'},
{'id': 'L21', 'text' : 'Acknowledgement Letter for Minor Grant / KG-cum-CCC (Repeated Applications)','enable':'01'},
{'id': 'L22', 'text' : 'Vetting of KG-cum-CCC (Email to EDB and FCW)','enable':'01'},
{'id': 'L23', 'text' : 'Approval Letter for Major Grant (From $500,000 to below $5,000,000)','enable':'01'},
{'id': 'L24', 'text' : 'Approval Letter for Major Works and F&E','enable':'01'},
{'id': 'L25', 'text' : 'Approval Letter for Minor Works and F&E','enable':'01'},
{'id': 'L26', 'text' : 'Approval Letter for Minor Works','enable':'01'},
{'id': 'L27', 'text' : 'Approval Letter for F&E','enable':'01'},
{'id': 'L28', 'text' : 'Approval Letter for Projects with Estimated Professional Fees exceeding $500,000','enable':'01'},
{'id': 'L29', 'text' : 'Approval Letter for Projects with Total Cost of Works over $10 Million (CEO)','enable':'01'},
{'id': 'L30', 'text' : 'Approval Letter for F&E (Laundry Equipment)','enable':'01'},
{'id': 'L31', 'text' : 'Approval Letter for F&E (Purchase for In-Situ Expansion)','enable':'01'},
{'id': 'L32', 'text' : 'Approval Letter for Bidding of AP','enable':'01'},
{'id': 'L33', 'text' : 'Approval Letter for Appointment of AP','enable':'01'},
{'id': 'L34', 'text' : 'Approval Letter for Inviting Tenders','enable':'01'},
{'id': 'L35', 'text' : 'Approval Letter for Awarding Tender','enable':'01'},
{'id': 'L36', 'text' : 'Approval Letter for Awarding Tender (With Supplementary Grant)','enable':'01'},
{'id': 'L37', 'text' : 'Approval for EOT','enable':'01'},
{'id': 'L38', 'text' : 'Acknowledgement Letter for Vehicle','enable':'01'},
{'id': 'L39', 'text' : 'Memo to Service Branch (New Vehicle)','enable':'01'},
{'id': 'L40', 'text' : 'Memo to Service Branch (Replacement of Vehicle)','enable':'01'},
{'id': 'L41', 'text' : 'Memo to EMSD (New Vehicle)','enable':'01'},
{'id': 'L42', 'text' : 'Memo to EMSD (Replacement of Vehicle)','enable':'01'},
{'id': 'L43', 'text' : 'Approval Letter for Vehicle (LPG PLB)','enable':'01'},
{'id': 'L44', 'text' : 'Approval Letter for Vehicle (Other than LPG PLB)','enable':'01'},
{'id': 'L45', 'text' : 'Award of Tender for Vehicle (New Vehicle with Relaxed Tender)','enable':'01'},
{'id': 'L46', 'text' : 'Award of Tender for Vehicle (Replacement of Vehicle with Relaxed Tender)','enable':'01'},
{'id': 'L47', 'text' : 'Memo to FSTB for Approval of Accepting Donation','enable':'01'},
{'id': 'L48', 'text' : 'Letter on Retaining Donations for Facilities Upgrading','enable':'01'},
{'id': 'L49', 'text' : 'Approval Letter for Vehicle (Replacement of Enro II Diesel Vehicles_LF withheld)','enable':'01'},
{'id': 'L50', 'text' : 'Approval Letter for F&E (Contract RCHE)','enable':'01'},
{'id': 'L51', 'text' : 'Email to Service Branch for Approval of Naming After','enable':'01'}
];

var dataLetterVersion = [
{"version":"1.0","description":"Approval Letter for Pilot Scheme (Home Care Services for Severe Disabilities)","active":"02"},
{"version":"1.1","description":"Approval Letter for Pilot Scheme (Home Care Services for Severe Disabilities) as at 13/14","active":"01"}
];

var dataPspAccountSummary = [{
    "recId": "1",
    "engChiOrgName": "ACCA Charitable Foundation Limited 沒有中文註冊名稱",
    "fileRef": "(2012)379",
    "processingOfficer": "EA(LF)SD1",
    "eventStartDate": "13/01/2013",
    "eventEndDate": "13/01/2013",
    "permitNum": "2013/005/1",
    "fundRaisingPurpose": "To raise funds for financing projects for 3 selected beneficiary organisations: 1) Jubilee Ministries Limited 2) Project Concern Hong Kong & 3) Otic Foundation Limited. Any surplus generated from the event after deducting the donation and other necessary expenses, will be transferred to ACCA Charitable Foundation Limited for future donations to other selected organisations",
    "FundUsed": "",
    "DocSubmission": "",
    "SubmissionDueDate": "",
    "FirstReminderIssueDate": "",
    "FirstReminderDeadline": "",
    "SecondReminderIssueDate": "",
    "SecondReminderDeadline": "",
    "AuditedReportReceivedDate": "",
    "PublicationReceivedDate": "",
    "OfficialReceiptReceivedDate": "",
    "NewspaperCuttingReceivedDate": "",
    "DocReceivedRemark": "",
    "GrossProceed": "",
    "Expenditure": "",
    "NetProceed": "",
    "OrgAnnualIncome": "",
    "Percent": "",
    "SanctionListIndicator": "",
    "QualifyOpinionIndicator": "",
    "QualityOpinionDetail": "",
    "WithholdingListIndicator": "",
    "ArCheckIndicator": "",
    "PublicationCheckIndicator": "",
    "OfficialReceiptCheckIndicator": "",
    "NewspaperCuttingCheckIndicator": "",
    "DocRemark": ""
}, {
    "recId": "2",
    "engChiOrgName": "Hong Kong Youth Hostels Association 香港青年旅舍協會",
    "fileRef": "(2012)401",
    "processingOfficer": "EA(LF)SD1",
    "eventStartDate": "20/01/2013",
    "eventEndDate": "20/01/2013",
    "permitNum": "2013/014/1",
    "fundRaisingPurpose": "籌款用作「香港青年旅舍協會」營運及舉辦各項活動",
    "FundUsed": "",
    "DocSubmission": "",
    "SubmissionDueDate": "",
    "FirstReminderIssueDate": "",
    "FirstReminderDeadline": "",
    "SecondReminderIssueDate": "",
    "SecondReminderDeadline": "",
    "AuditedReportReceivedDate": "",
    "PublicationReceivedDate": "",
    "OfficialReceiptReceivedDate": "",
    "NewspaperCuttingReceivedDate": "",
    "DocReceivedRemark": "",
    "GrossProceed": "",
    "Expenditure": "",
    "NetProceed": "",
    "OrgAnnualIncome": "",
    "Percent": "",
    "SanctionListIndicator": "",
    "QualifyOpinionIndicator": "",
    "QualityOpinionDetail": "",
    "WithholdingListIndicator": "",
    "FileRefLmNum": "",
    "ArCheckIndicator": "",
    "PublicationCheckIndicator": "",
    "OfficialReceiptCheckIndicator": "",
    "NewspaperCuttingCheckIndicator": "",
    "DocRemark": ""
}, {
    "recId": "3",
    "engChiOrgName": "The Yuen Yuen Institute  圓玄學院",
    "fileRef": "(2012)403",
    "processingOfficer": "EO(LF)MS",
    "eventStartDate": "13/01/2013",
    "eventEndDate": "13/01/2013",
    "permitNum": "2013/001/1",
    "fundRaisingPurpose": "籌款用作「圓玄學院」特別學習需要學生服務及老年痴呆症服務的經費",
    "FundUsed": "",
    "DocSubmission": "",
    "SubmissionDueDate": "",
    "FirstReminderIssueDate": "",
    "FirstReminderDeadline": "",
    "SecondReminderIssueDate": "",
    "SecondReminderDeadline": "",
    "AuditedReportReceivedDate": "",
    "PublicationReceivedDate": "",
    "OfficialReceiptReceivedDate": "",
    "NewspaperCuttingReceivedDate": "",
    "DocReceivedRemark": "",
    "GrossProceed": "",
    "Expenditure": "",
    "NetProceed": "",
    "OrgAnnualIncome": "",
    "Percent": "",
    "SanctionListIndicator": "",
    "QualifyOpinionIndicator": "",
    "QualityOpinionDetail": "",
    "WithholdingListIndicator": "",
    "FileRefLmNum": "",
    "ArCheckIndicator": "",
    "PublicationCheckIndicator": "",
    "OfficialReceiptCheckIndicator": "",
    "NewspaperCuttingCheckIndicator": "",
    "DocRemark": ""
}, {
    "recId": "4",
    "engChiOrgName": "The Yuen Yuen Institute  圓玄學院",
    "fileRef": "(2012)404",
    "processingOfficer": "EO(LF)MS",
    "eventStartDate": "19/01/2013",
    "eventEndDate": "17/03/2013",
    "permitNum": "2013/004/1",
    "fundRaisingPurpose": "籌款用作「圓玄學院」特別學習需要學生服務及老年痴呆症服務的經費",
    "FundUsed": "",
    "DocSubmission": "",
    "SubmissionDueDate": "",
    "FirstReminderIssueDate": "",
    "FirstReminderDeadline": "",
    "SecondReminderIssueDate": "",
    "SecondReminderDeadline": "",
    "AuditedReportReceivedDate": "",
    "PublicationReceivedDate": "",
    "OfficialReceiptReceivedDate": "",
    "NewspaperCuttingReceivedDate": "",
    "DocReceivedRemark": "",
    "GrossProceed": "",
    "Expenditure": "",
    "NetProceed": "",
    "OrgAnnualIncome": "",
    "Percent": "",
    "SanctionListIndicator": "",
    "QualifyOpinionIndicator": "",
    "QualityOpinionDetail": "",
    "WithholdingListIndicator": "",
    "FileRefLmNum": "",
    "ArCheckIndicator": "",
    "PublicationCheckIndicator": "",
    "OfficialReceiptCheckIndicator": "",
    "NewspaperCuttingCheckIndicator": "",
    "DocRemark": ""
}, {
    "recId": "5",
    "engChiOrgName": "Pok Oi Hospital 博愛醫院",
    "fileRef": "(2012)424",
    "processingOfficer": "EA(LF)SD1",
    "eventStartDate": "19/01/2013",
    "eventEndDate": "24/02/2013",
    "permitNum": "2013/003/1",
    "fundRaisingPurpose": "籌款用作擴展「博愛醫院」各項社會服務",
    "FundUsed": "",
    "DocSubmission": "",
    "SubmissionDueDate": "",
    "FirstReminderIssueDate": "",
    "FirstReminderDeadline": "",
    "SecondReminderIssueDate": "",
    "SecondReminderDeadline": "",
    "AuditedReportReceivedDate": "",
    "PublicationReceivedDate": "",
    "OfficialReceiptReceivedDate": "",
    "NewspaperCuttingReceivedDate": "",
    "DocReceivedRemark": "",
    "GrossProceed": "",
    "Expenditure": "",
    "NetProceed": "",
    "OrgAnnualIncome": "",
    "Percent": "",
    "SanctionListIndicator": "",
    "QualifyOpinionIndicator": "",
    "QualityOpinionDetail": "",
    "WithholdingListIndicator": "",
    "FileRefLmNum": "",
    "ArCheckIndicator": "",
    "PublicationCheckIndicator": "",
    "OfficialReceiptCheckIndicator": "",
    "NewspaperCuttingCheckIndicator": "",
    "DocRemark": ""
}, {
    "recId": "6",
    "engChiOrgName": "Hong Kong Buddhist Hospital 香港佛教醫院",
    "fileRef": "(2012)426",
    "processingOfficer": "EO(LF)MS",
    "eventStartDate": "01/02/2013",
    "eventEndDate": "01/02/2013",
    "permitNum": "2013/019/1",
    "fundRaisingPurpose": "To raise funds for the purchase of medical drugs, instruments and equipment, office equipment, the improvement of hospital premises, and supporting patient related activities.",
    "FundUsed": "",
    "DocSubmission": "",
    "SubmissionDueDate": "",
    "FirstReminderIssueDate": "",
    "FirstReminderDeadline": "",
    "SecondReminderIssueDate": "",
    "SecondReminderDeadline": "",
    "AuditedReportReceivedDate": "",
    "PublicationReceivedDate": "",
    "OfficialReceiptReceivedDate": "",
    "NewspaperCuttingReceivedDate": "",
    "DocReceivedRemark": "",
    "GrossProceed": "",
    "Expenditure": "",
    "NetProceed": "",
    "OrgAnnualIncome": "",
    "Percent": "",
    "SanctionListIndicator": "",
    "QualifyOpinionIndicator": "",
    "QualityOpinionDetail": "",
    "WithholdingListIndicator": "",
    "FileRefLmNum": "",
    "ArCheckIndicator": "",
    "PublicationCheckIndicator": "",
    "OfficialReceiptCheckIndicator": "",
    "NewspaperCuttingCheckIndicator": "",
    "DocRemark": ""
}, {
    "recId": "7",
    "engChiOrgName": "Suicide Prevention Services Limited 生命熱線有限公司",
    "fileRef": "(2012)427",
    "processingOfficer": "EA(LF)SD1",
    "eventStartDate": "13/01/2013",
    "eventEndDate": "13/01/2013",
    "permitNum": "2013/009/1",
    "fundRaisingPurpose": " To raise funds for services expenses",
    "FundUsed": "",
    "DocSubmission": "",
    "SubmissionDueDate": "",
    "FirstReminderIssueDate": "",
    "FirstReminderDeadline": "",
    "SecondReminderIssueDate": "",
    "SecondReminderDeadline": "",
    "AuditedReportReceivedDate": "",
    "PublicationReceivedDate": "",
    "OfficialReceiptReceivedDate": "",
    "NewspaperCuttingReceivedDate": "",
    "DocReceivedRemark": "",
    "GrossProceed": "",
    "Expenditure": "",
    "NetProceed": "",
    "OrgAnnualIncome": "",
    "Percent": "",
    "SanctionListIndicator": "",
    "QualifyOpinionIndicator": "",
    "QualityOpinionDetail": "",
    "WithholdingListIndicator": "",
    "FileRefLmNum": "",
    "ArCheckIndicator": "",
    "PublicationCheckIndicator": "",
    "OfficialReceiptCheckIndicator": "",
    "NewspaperCuttingCheckIndicator": "",
    "DocRemark": ""
}, {
    "recId": "8",
    "engChiOrgName": "Shepherd Community Church 基督教牧鄰教會",
    "fileRef": "(2012)433",
    "processingOfficer": "EOII(FC)",
    "eventStartDate": "06/01/2013",
    "eventEndDate": "06/03/2013",
    "permitNum": "2013/039/1",
    "fundRaisingPurpose": "籌款用作教會常費及是次場地租金",
    "FundUsed": "",
    "DocSubmission": "",
    "SubmissionDueDate": "",
    "FirstReminderIssueDate": "",
    "FirstReminderDeadline": "",
    "SecondReminderIssueDate": "",
    "SecondReminderDeadline": "",
    "AuditedReportReceivedDate": "",
    "PublicationReceivedDate": "",
    "OfficialReceiptReceivedDate": "",
    "NewspaperCuttingReceivedDate": "",
    "DocReceivedRemark": "",
    "GrossProceed": "",
    "Expenditure": "",
    "NetProceed": "",
    "OrgAnnualIncome": "",
    "Percent": "",
    "SanctionListIndicator": "",
    "QualifyOpinionIndicator": "",
    "QualityOpinionDetail": "",
    "WithholdingListIndicator": "",
    "FileRefLmNum": "",
    "ArCheckIndicator": "",
    "PublicationCheckIndicator": "",
    "OfficialReceiptCheckIndicator": "",
    "NewspaperCuttingCheckIndicator": "",
    "DocRemark": ""
}, {
    "recId": "9",
    "engChiOrgName": "Chun Oi Tong Limited 頌愛堂有限公司",
    "fileRef": "(2012)435",
    "processingOfficer": "EO(LF)MS",
    "eventStartDate": "21/01/2013",
    "eventEndDate": "31/01/2013",
    "permitNum": "2013/012/1",
    "fundRaisingPurpose": "籌款用作「頌愛堂有限公司」幫助有需要人士如獨居老人及單親家庭",
    "FundUsed": "",
    "DocSubmission": "",
    "SubmissionDueDate": "",
    "FirstReminderIssueDate": "",
    "FirstReminderDeadline": "",
    "SecondReminderIssueDate": "",
    "SecondReminderDeadline": "",
    "AuditedReportReceivedDate": "",
    "PublicationReceivedDate": "",
    "OfficialReceiptReceivedDate": "",
    "NewspaperCuttingReceivedDate": "",
    "DocReceivedRemark": "",
    "GrossProceed": "",
    "Expenditure": "",
    "NetProceed": "",
    "OrgAnnualIncome": "",
    "Percent": "",
    "SanctionListIndicator": "",
    "QualifyOpinionIndicator": "",
    "QualityOpinionDetail": "",
    "WithholdingListIndicator": "",
    "FileRefLmNum": "",
    "ArCheckIndicator": "",
    "PublicationCheckIndicator": "",
    "OfficialReceiptCheckIndicator": "",
    "NewspaperCuttingCheckIndicator": "",
    "DocRemark": ""
}, {
    "recId": "10",
    "engChiOrgName": "Project Orbis International, Inc 沒有中文註冊名稱",
    "fileRef": "(2012)438",
    "processingOfficer": "EA(LF)SD1",
    "eventStartDate": "08/01/2013",
    "eventEndDate": "03/03/2013",
    "permitNum": "2013/002/1",
    "fundRaisingPurpose": "To raise funds for operating expenses for sight saving worldwide",
    "FundUsed": "",
    "DocSubmission": "",
    "SubmissionDueDate": "",
    "FirstReminderIssueDate": "",
    "FirstReminderDeadline": "",
    "SecondReminderIssueDate": "",
    "SecondReminderDeadline": "",
    "AuditedReportReceivedDate": "",
    "PublicationReceivedDate": "",
    "OfficialReceiptReceivedDate": "",
    "NewspaperCuttingReceivedDate": "",
    "DocReceivedRemark": "",
    "GrossProceed": "",
    "Expenditure": "",
    "NetProceed": "",
    "OrgAnnualIncome": "",
    "Percent": "",
    "SanctionListIndicator": "",
    "QualifyOpinionIndicator": "",
    "QualityOpinionDetail": "",
    "WithholdingListIndicator": "",
    "FileRefLmNum": "",
    "ArCheckIndicator": "",
    "PublicationCheckIndicator": "",
    "OfficialReceiptCheckIndicator": "",
    "NewspaperCuttingCheckIndicator": "",
    "DocRemark": ""
}];

var dataFdAccountSummary = [{
    "recId": "1",
    "FlagDay": "12/04/2014",
    "Twr": "Hong Kong Island",
    "permitNum": "R002",
    "fileRef": "SWD117/2/2005/49(2014/15)",
    "engChiOrgName": "Heep Hong Society 協康會",
    "DocSubmission": "",
    "SubmissionDueDate": "",
    "FirstReminderIssueDate": "",
    "FirstReminderDeadline": "",
    "SecondReminderIssueDate": "",
    "SecondReminderDeadline": "",
    "AuditedReportReceivedDate": "",
    "NewspaperCuttingReceivedDate": "",
    "DocReceivedRemark": "",
    "DocRemark": "",
    "StreetCollection": "",
    "GrossProceed": "",
    "Expenditure": "",
    "NetProceed": "",
    "Percent": "",
    "NewspaperPublishingDate": "",
    "PledgingAmount": "",
    "AcknowledgeReceiveDate": "",
    "AfsCheckIndicator": "",
    "RequestFollowUpIndicator": "",
    "AfsResubmitIndicator": "",
    "AfsReminderIndicator": "",
    "Remark": ""
}, {
    "recId": "2",
    "FlagDay": "12/04/2014",
    "Twr": "Kowloon",
    "permitNum": "R037",
    "fileRef": "SWD033/2/2005/49(2014/15)",
    "engChiOrgName": "Employees' Safety, Training & Rehabilitation Services Limited 職安培訓復生會有限公司",
    "DocSubmission": "",
    "SubmissionDueDate": "",
    "FirstReminderIssueDate": "",
    "FirstReminderDeadline": "",
    "SecondReminderIssueDate": "",
    "SecondReminderDeadline": "",
    "AuditedReportReceivedDate": "",
    "NewspaperCuttingReceivedDate": "",
    "DocReceivedRemark": "",
    "DocRemark": "",
    "StreetCollection": "",
    "GrossProceed": "",
    "Expenditure": "",
    "NetProceed": "",
    "Percent": "",
    "NewspaperPublishingDate": "",
    "PledgingAmount": "",
    "AcknowledgeReceiveDate": "",
    "AfsCheckIndicator": "",
    "RequestFollowUpIndicator": "",
    "AfsResubmitIndicator": "",
    "AfsReminderIndicator": "",
    "Remark": ""
}, {
    "recId": "3",
    "FlagDay": "12/04/2014",
    "Twr": "New Territories",
    "permitNum": "R079",
    "fileRef": "SWD052/2/2005/49(2014/15)  ",
    "engChiOrgName": "Society for the Welfare of the Autistic Persons 自閉症人士福利促進會",
    "DocSubmission": "",
    "SubmissionDueDate": "",
    "FirstReminderIssueDate": "",
    "FirstReminderDeadline": "",
    "SecondReminderIssueDate": "",
    "SecondReminderDeadline": "",
    "AuditedReportReceivedDate": "",
    "NewspaperCuttingReceivedDate": "",
    "DocReceivedRemark": "",
    "DocRemark": "",
    "StreetCollection": "",
    "GrossProceed": "",
    "Expenditure": "",
    "NetProceed": "",
    "Percent": "",
    "NewspaperPublishingDate": "",
    "PledgingAmount": "",
    "AcknowledgeReceiveDate": "",
    "AfsCheckIndicator": "",
    "RequestFollowUpIndicator": "",
    "AfsResubmitIndicator": "",
    "AfsReminderIndicator": "",
    "Remark": ""
}, {
    "recId": "4",
    "FlagDay": "19/04/2014",
    "Twr": "Territory-wide",
    "permitNum": "T015",
    "fileRef": "SWD060/2/2005/49(2014/15)",
    "engChiOrgName": "Hong Kong Student Aid Society 香港學生輔助會",
    "DocSubmission": "",
    "SubmissionDueDate": "",
    "FirstReminderIssueDate": "",
    "FirstReminderDeadline": "",
    "SecondReminderIssueDate": "",
    "SecondReminderDeadline": "",
    "AuditedReportReceivedDate": "",
    "NewspaperCuttingReceivedDate": "",
    "DocReceivedRemark": "",
    "DocRemark": "",
    "StreetCollection": "",
    "GrossProceed": "",
    "Expenditure": "",
    "NetProceed": "",
    "Percent": "",
    "NewspaperPublishingDate": "",
    "PledgingAmount": "",
    "AcknowledgeReceiveDate": "",
    "AfsCheckIndicator": "",
    "RequestFollowUpIndicator": "",
    "AfsResubmitIndicator": "",
    "AfsReminderIndicator": "",
    "Remark": ""
}, {
    "recId": "5",
    "FlagDay": "23/04/2014",
    "Twr": "Territory-wide",
    "permitNum": "T024",
    "fileRef": "SWD155/2/2005/49(2014/15)",
    "engChiOrgName": "Hong Kong Association of the Deaf 香港聾人協進會",
    "DocSubmission": "",
    "SubmissionDueDate": "",
    "FirstReminderIssueDate": "",
    "FirstReminderDeadline": "",
    "SecondReminderIssueDate": "",
    "SecondReminderDeadline": "",
    "AuditedReportReceivedDate": "",
    "NewspaperCuttingReceivedDate": "",
    "DocReceivedRemark": "",
    "DocRemark": "",
    "StreetCollection": "",
    "GrossProceed": "",
    "Expenditure": "",
    "NetProceed": "",
    "Percent": "",
    "NewspaperPublishingDate": "",
    "PledgingAmount": "",
    "AcknowledgeReceiveDate": "",
    "AfsCheckIndicator": "",
    "RequestFollowUpIndicator": "",
    "AfsResubmitIndicator": "",
    "AfsReminderIndicator": "",
    "Remark": ""
}, {
    "recId": "6",
    "FlagDay": "26/04/2014",
    "Twr": "Hong Kong Island",
    "permitNum": "R001",
    "fileRef": "SWD129/2/2005/49(2014/15)",
    "engChiOrgName": "The Conservancy Association 長春社",
    "DocSubmission": "",
    "SubmissionDueDate": "",
    "FirstReminderIssueDate": "",
    "FirstReminderDeadline": "",
    "SecondReminderIssueDate": "",
    "SecondReminderDeadline": "",
    "AuditedReportReceivedDate": "",
    "NewspaperCuttingReceivedDate": "",
    "DocReceivedRemark": "",
    "DocRemark": "",
    "StreetCollection": "",
    "GrossProceed": "",
    "Expenditure": "",
    "NetProceed": "",
    "Percent": "",
    "NewspaperPublishingDate": "",
    "PledgingAmount": "",
    "AcknowledgeReceiveDate": "",
    "AfsCheckIndicator": "",
    "RequestFollowUpIndicator": "",
    "AfsResubmitIndicator": "",
    "AfsReminderIndicator": "",
    "Remark": ""
}, {
    "recId": "7",
    "FlagDay": "26/04/2014",
    "Twr": "Kowloon",
    "permitNum": "R018",
    "fileRef": "SWD019/2/2005/49(2014/15)",
    "engChiOrgName": "Hong Kong Society for the Protection of Children 香港保護兒童會",
    "DocSubmission": "",
    "SubmissionDueDate": "",
    "FirstReminderIssueDate": "",
    "FirstReminderDeadline": "",
    "SecondReminderIssueDate": "",
    "SecondReminderDeadline": "",
    "AuditedReportReceivedDate": "",
    "NewspaperCuttingReceivedDate": "",
    "DocReceivedRemark": "",
    "DocRemark": "",
    "StreetCollection": "",
    "GrossProceed": "",
    "Expenditure": "",
    "NetProceed": "",
    "Percent": "",
    "NewspaperPublishingDate": "",
    "PledgingAmount": "",
    "AcknowledgeReceiveDate": "",
    "AfsCheckIndicator": "",
    "RequestFollowUpIndicator": "",
    "AfsResubmitIndicator": "",
    "AfsReminderIndicator": "",
    "Remark": ""
}, {
    "recId": "8",
    "FlagDay": "26/04/2014",
    "Twr": "New Territories",
    "permitNum": "R082",
    "fileRef": "SWD042/2/2005/49(2014/15)",
    "engChiOrgName": "Tseung Kwan O Sion Church Limited 將軍澳基督教錫安堂有限公司",
    "DocSubmission": "",
    "SubmissionDueDate": "",
    "FirstReminderIssueDate": "",
    "FirstReminderDeadline": "",
    "SecondReminderIssueDate": "",
    "SecondReminderDeadline": "",
    "AuditedReportReceivedDate": "",
    "NewspaperCuttingReceivedDate": "",
    "DocReceivedRemark": "",
    "DocRemark": "",
    "StreetCollection": "",
    "GrossProceed": "",
    "Expenditure": "",
    "NetProceed": "",
    "Percent": "",
    "NewspaperPublishingDate": "",
    "PledgingAmount": "",
    "AcknowledgeReceiveDate": "",
    "AfsCheckIndicator": "",
    "RequestFollowUpIndicator": "",
    "AfsResubmitIndicator": "",
    "AfsReminderIndicator": "",
    "Remark": ""
}, {
    "recId": "9",
    "FlagDay": "03/05/2014",
    "Twr": "Hong Kong Island",
    "permitNum": "R060",
    "fileRef": "SWD149/2/2005/49(2014/15)",
    "engChiOrgName": "Hong Kong Alzheimer's Disease Association 香港認知障礙症協會",
    "DocSubmission": "",
    "SubmissionDueDate": "",
    "FirstReminderIssueDate": "",
    "FirstReminderDeadline": "",
    "SecondReminderIssueDate": "",
    "SecondReminderDeadline": "",
    "AuditedReportReceivedDate": "",
    "NewspaperCuttingReceivedDate": "",
    "DocReceivedRemark": "",
    "DocRemark": "",
    "StreetCollection": "",
    "GrossProceed": "",
    "Expenditure": "",
    "NetProceed": "",
    "Percent": "",
    "NewspaperPublishingDate": "",
    "PledgingAmount": "",
    "AcknowledgeReceiveDate": "",
    "AfsCheckIndicator": "",
    "RequestFollowUpIndicator": "",
    "AfsResubmitIndicator": "",
    "AfsReminderIndicator": "",
    "Remark": ""
}, {
    "recId": "10",
    "FlagDay": "03/05/2014",
    "Twr": "Kowloon",
    "permitNum": "R040",
    "fileRef": "SWD048/2/2005/49(2014/15)",
    "engChiOrgName": "Hong Kong Rehabilitation Power 香港復康力量",
    "DocSubmission": "",
    "SubmissionDueDate": "",
    "FirstReminderIssueDate": "",
    "FirstReminderDeadline": "",
    "SecondReminderIssueDate": "",
    "SecondReminderDeadline": "",
    "AuditedReportReceivedDate": "",
    "NewspaperCuttingReceivedDate": "",
    "DocReceivedRemark": "",
    "DocRemark": "",
    "StreetCollection": "",
    "GrossProceed": "",
    "Expenditure": "",
    "NetProceed": "",
    "Percent": "",
    "NewspaperPublishingDate": "",
    "PledgingAmount": "",
    "AcknowledgeReceiveDate": "",
    "AfsCheckIndicator": "",
    "RequestFollowUpIndicator": "",
    "AfsResubmitIndicator": "",
    "AfsReminderIndicator": "",
    "Remark": ""
}];