<template>
    <div class="center">
        <nav class="navbar navbar-expand-sm bg-dark navbar-dark fixed-top">
            <div class="container-fluid">
                <a class="navbar-brand pointer" @click="SetPage('HomePage')">
                    <img src="smallcoins2.png" style="margin: 0px 20px;">
                    <h4 class="inline">Not A Lottery</h4>
                </a>
                <div class="collapse navbar-collapse d-flex justify-content-center my-navbar-btns" id="collapsibleNavbar">
                    <button :class="['btn', 'btn-outline-secondary', 'white', page === 'LottoPage' ? 'highlight-btn' : '']"
                            type="button"
                            @click="SetPage('LottoPage')"
                            style="margin-right: 20px;">
                        Lottery
                    </button>
                    <button :class="['btn', 'btn-outline-secondary', 'white', page === 'PowerballPage' ? 'highlight-btn' : '']" 
                            type="button"  
                            @click="SetPage('PowerballPage')">
                        Powerball
                    </button>
                </div>
                <div class="collapse navbar-collapse d-flex justify-content-end my-navbar-btns" id="collapsibleNavbar">
                    <button v-if="currentAddress === null" @click="ConnectMetaMask()" type="button" class="btn btn-dark connect-btn">Connect MetaMask</button>
                    <button v-else type="button" class="btn btn-dark connected-btn">{{ currentAddressDisplay }}</button>
                </div>
            </div>
        </nav>
        
        <div v-if="page === 'HomePage'">
            <div class="page-header" v-show="showPage">
                <div>
                    <h3>Invite your friends! The more that play, the bigger the prize!</h3>
                    <br>
                    <HowToPlay></HowToPlay>
                </div>
                <br>
                <div v-if="metaMaskDownloaded === false || chainId !== pulseChainId || currentAddress === null">
                    <h1 style="color:orangered">You must be on the PulseChain network to play this game!</h1>
                    <a v-show="metaMaskDownloaded === false" href="https://metamask.io/download/" target="_blank" class="download-mm-btn">Download MetaMask?</a>
                    <button v-show="metaMaskDownloaded !== false && currentAddress === null" 
                        type="button" class="btn btn-warning" @click="ConnectMetaMask()">
                        Connect MetaMask
                    </button>
                    <button v-show="metaMaskDownloaded !== false && currentAddress !== null && chainId !== pulseChainId" 
                        type="button" class="btn btn-warning" @click="AddPulseChainToMetaMaskOrSwitchToPulseChain()">
                        Switch to PulseChain?
                    </button>
                    <br>
                    <br>
                    <br>
                </div>
                <div v-if="statistics !== null"><h4>Total winnings paid out so far: {{ statistics.totalPrizeMoneyDisplay }} PLS</h4></div>
                <br>
                <div v-if="statistics !== null"><h4>Total tickets sold so far: {{ statistics.totalNumberPlayersDisplay }}</h4></div>
                <br>
                <div v-if="showMyPrizeMoney">
                    <h1 style="color: lawngreen;">You've won {{ winningsTotalPulse }} of PLS!!! Congrats!!!</h1>
                </div>
                <div>
                    <button type="button" class="btn btn-success" @click="ClaimWinnings()" :disabled="canClaimPrize">Claim All Winnings!!!</button>
                </div>
                <br />
                <br>
                <div>
                    <p>How do you know we are legit? Here's the link to the code! Go look for yourself. It's open 
                        to the public. The winners are chosen fairly. 
                        <a href="https://github.com/jason-rice/NotALotteryGame" style="color: deepskyblue;">
                            Not A Lottery GitHub Repository
                        </a>
                    </p>
                    <p>This project is an attempt to bring adoption and awareness to PulseChain. This is supposed 
                        to be a fun game that countributes to this community that we love.
                    </p>
                    <p>Who are we? We joined the crypto wave back in 2020. We first got into bitcoin and ethereum, 
                        like everyone else. We quickly discovered Richard Heart and the HEX ecosystem. We fell in 
                        love with the pricinples of this ecosystem. From decentralization, to freedom, to individual 
                        sovereignty and the live and the let live mentality. We believe in the golden rule. Treat 
                        others as you wish to be treated. We ditched the other coins we owned and aped into HEX. We 
                        sacrificed for PulseX. We were there day 1 of the PulseChain launch. We've been quitely 
                        watching and participating in the background. We finally decided we could contribute to this 
                        PulseChain ecosystem in a positive and meaningful way. We've seen rug pulls and bad projects 
                        get hacked. Lots of people have lost a lot of money on sub par projects. We can do better.
                    </p>
                    <p>The head developer has been a professional senior software engineer for nearly a decade now. 
                        He works in the Microsoft stack and with Vue.js primarily. He has worked with many projects, 
                        but there's one in particular that should stand out to anyone concerning this project. He 
                        built the functionality on the front end, backend, and in the database, for the giving plaform 
                        for a large SASS company. More than $150 million dollars a year passes through the software he 
                        built, from companies and individuals worldwide. He custom built all of the code 
                        for his company to communicate with the payment providers.
                    </p>
                    <p>This is not our first rodeo! We have a lot more on the way. We're adding to this project and 
                        building another, exciting project, that we hope to launch soon!! Stay tuned! Have fun and 
                        enjoy our project!!!
                    </p>
                </div>
            </div>

        </div>
        
        <div v-if="page === 'LottoPage'">
            <div class="page-header" v-show="showPage">
                <p>These are the lotteries you can join. Just buy one or more tickets, for 20,000 PLS tokens each!</p>
                <p>When the time runs out, a winner is randomly chosen by the Microsoft c# random number generator.</p>
                <p>For proof that the winner is chosen fairly, you can look at this code that runs the lottery. <a href="https://github.com/jason-rice/NotALotteryGame/blob/master/webapi/RepeatingService.cs" style="color: deepskyblue;"> Proof of fairness</a></p>
                <div>
                    <h1 style="color: lawngreen;" v-if="youBoughtLottoTicket === true">
                        Congratulations!! You've entered the lottery!!
                    </h1>
                </div>
                <div v-if="showLottos" class="row row-cols-1 row-cols-sm-2 row-cols-md-2 row-cols-lg-3 row-cols-xl-4 g-4" style="padding: 5% 10%;">
                    <Lotto v-for="(t, index) in lottoTypes" :key="t"
                        :lottoType="lottoTypes[index]" 
                        :timeEnd="lottoTimes[index].dateAndTime"
                        :ticketsBoughtIncoming="ticketsBought[index]"
                        :totalTicketsBoughtIncoming="totalTicketsBought[index]"
                        :totalPlsIncoming="totalPlsList[index]"
                        :escrowAccountNumIncoming="escrowAccountNum"
                        :winners="lottoWinnerLists[index]"
                        @youBoughtLotto="BoughtLotto()"
                        @refreshLotto="RefreshLotto()"
                    />
                </div>
            </div>
        </div>

        <div v-if="page === 'PowerballPage'">
            <div class="page-header">
                <p>
                    The powerball will continue until one or more people guess the correct, randomly generated
                    numbers! Each round lasts one day.
                </p>
                <p>
                    You buy a ticket for a round. If your numbers are chosen, you win. If nobody wins, the prize money 
                    rolls over into the next round and the tickets are all removed.
                </p>
                <p>
                    Just like the powerball, you will need to buy a new ticket for each round, in order to win. This 
                    will continue until a winner is selected! Good luck!!!
                </p>
                <div>
                    <h1 style="color: lawngreen;" v-if="youBoughtPowerballTicket === true">
                        Congratulations!! You've entered the Powerball!!
                    </h1>
                </div>
                <div v-if="showPowerball" class="row" style="padding: 5% 10%;">
                    <Powerball  
                        :lottoType="powerballTypes[0]" 
                        :timeEnd="lottoTimes[9].dateAndTime"
                        :ticketsBoughtIncoming="ticketsBought[9]"
                        :totalTicketsBoughtIncoming="totalTicketsBought[9]"
                        :totalPlsIncoming="totalPlsList[9]"
                        :escrowAccountNumIncoming="escrowAccountNum"
                        :winners="powerballWinnerLists[0]"
                        @youBoughtPowerball="BoughtPowerball()"
                        @refreshPowerball="RefreshPowerball()"
                    />
                </div>
            </div>
        </div>
        
        <Disclaimer></Disclaimer>
    </div>
</template>

<script>
    import Lotto from './components/Lotto_Card.vue';
    import Powerball from './components/Powerball_Card.vue';
    import HowToPlay from './components/HowToPlay.vue';
    import Disclaimer from './components/Disclaimer.vue';
    
export default {
  name: 'App',
  data() {
        return {
            currentAddress: null,
            chainId: null,
            page: 'HomePage',
            showPage: false,
            pulseChainId: '0x171',
            lottoTimes: null,
            lottoWinnerLists: null,
            powerballWinnerLists: null,
            lottoTypes: [
                { id: 1, type: 'Two Minute Lotto' },
                { id: 2, type: 'Five Minute Lotto' },
                { id: 3, type: 'Thirty Minute Lotto' },
                { id: 4, type: 'Hourly Lotto' },
                // { id: 5, type: 'Two Hour Lotto' },
                // { id: 6, type: 'Six Hour Lotto' },
                // { id: 7, type: 'Twelve Hour Lotto' },
                { id: 8, type: 'Daily Lotto' },
                { id: 9, type: 'Weekly Lotto' },
                // { id: 10, type: 'Daily Powerball' },
            ],
            powerballTypes: [
                { id: 10, type: 'Daily Powerball' },
            ],
            ticketsBought: null,
            totalTicketsBought: null,
            totalPlsList: null,
            winningsTotalPulse: null,
            statistics: null,
            escrowAccountNum: null,
            youBoughtPowerballTicket: false,
            youBoughtLottoTicket: false,
        };
    },
  components: {
    Lotto,
    Powerball,
    HowToPlay,
    Disclaimer,
  },
  methods: {
    InitializePage() { // don't need currentAddress for these
        this.page = localStorage.getItem('notalotteryapp');
        if (this.page === null) {
            this.page = "HomePage";
            localStorage.setItem('notalotteryapp', "HomePage");
        }
        
        this.GetLottoTimes();
        this.GetLottoWinnerLists();
        this.GetPowerballWinnerLists();
        this.GetTicketsBought(); // returns an array of 0's if no currentAddress
        this.GetTotalTicketsBought();
        this.GetTotalPlsList();
        this.GetStatistics();
        this.GetEscrowAccountNum();

        let vm = this;
        setInterval(function () {
            vm.GetTotalPlsList();
            vm.GetStatistics();
            vm.GetTotalTicketsBought();
            vm.GetLottoWinnerLists();
            vm.GetPowerballWinnerLists();
        }, 10000);
        
        setInterval(() => {
            this.GetLottoTimes();
        }, 3500)
    },
    async AccountFound() {
        if (window.ethereum !== null && window.ethereum !== undefined) {
            this.currentAddress = window.ethereum.selectedAddress;
            this.chainId = window.ethereum.chainId;

            window.ethereum.on('chainChanged', (chainId) => this.reload(chainId));
            window.ethereum.on('accountsChanged', (account) => this.reload(account));

            if (this.currentAddress !== null) {
                this.GetWinnings(this.currentAddress);
                this.GetTicketsBought(this.currentAddress);
            } 
        }
        this.showPage = true;
    },
    SetPage(value) {
        this.page = value;
        localStorage.setItem('notalotteryapp', value);
    },
    async AddPulseChainToMetaMaskOrSwitchToPulseChain() {
        try {
            await window.ethereum.request({
                "method": "wallet_switchEthereumChain",
                "params": [ { "chainId": this.pulseChainId } ]
            });
        } catch (switchError) {
            // This error code indicates that the chain has not been added to MetaMask.
            console.log('chain has not been added to this MetaMask wallet');
            if (switchError.code === 4902 || switchError.code === -32603) {
                try {
                    await window.ethereum.request({
                        method: 'wallet_addEthereumChain',
                        params: [
                            {
                                "blockExplorerUrls": [
                                    "https://otter.PulseChain.com"
                                ],
                                chainId: this.pulseChainId,
                                chainName: 'PulseChain',
                                rpcUrls: [
                                    'https://rpc.pulsechain.com'
                                ],
                                "nativeCurrency": {
                                    "name": "PLS",
                                    "symbol": "PLS",
                                    "decimals": 18
                                }
                            },
                        ],
                    });
                    this.reload();
                } catch (addError) {
                // handle "add" chain error
                    console.log('handle "add" chain error');
                }
            }
            // handle other "switch" errors
            console.log('handle other "switch" errors');
        }
    },
    async SwitchToPulseChain() {
        await window.ethereum.request({
            "method": "wallet_switchEthereumChain",
            "params": [ { "chainId": this.pulseChainId } ]
        });
    },
    async ConnectMetaMask() {
        const accounts = await window.ethereum.request({ method: 'eth_requestAccounts' })
            .catch((err) => {
            if (err.code === 4001) {
                // EIP-1193 userRejectedRequest error
                // If this happens, the user rejected the connection request.
                console.log('Please connect to MetaMask.');
            } else {
                console.error(err);
            }
            });
        this.currentAddress = accounts[0];
    },
    async GetLottoTimes() {
        await fetch('api/NotALottery/GetLottoTimes', {
            method: 'Get',
            headers: { 'Content-type': 'application/json; charset=UTF-8' }
        })
        .then(r => r.json())
        .then(json => {
            if (json.status !== 400) {
                this.lottoTimes = json;
            } else {
                this.lottoTimes = null;
            }
            return;
        });
    },
    async GetLottoWinnerLists() {
        await fetch('api/NotALottery/GetLottoWinnerLists', {
            method: 'Get',
            headers: { 'Content-type': 'application/json; charset=UTF-8' }
        })
        .then(r => r.json())
        .then(json => {
            if (json.status !== 400) {
                this.lottoWinnerLists = json;
            } else {
                this.lottoWinnerLists = null;
            }
            return;
        });
    },
    async GetPowerballWinnerLists() {
        await fetch('api/NotALottery/GetPowerballWinnerLists', {
            method: 'Get',
            headers: { 'Content-type': 'application/json; charset=UTF-8' }
        })
        .then(r => r.json())
        .then(json => {
            if (json.status !== 400) {
                this.powerballWinnerLists = json;
            } else {
                this.powerballWinnerLists = null;
            }
            return;
        });
    },
    async GetTotalPlsList() {
        await fetch('api/NotALottery/GetTotalPlsList', {
            method: 'Get',
            headers: { 'Content-type': 'application/json; charset=UTF-8' }
        })
        .then(r => r.json())
        .then(json => {
            this.totalPlsList = json;
            return;
        });
    },
    async GetWinnings(val) {
        let payload = { AccountNum: val };

        await fetch('api/NotALottery/GetWinnings', {
            method: 'POST',
            body: JSON.stringify(payload),
            headers: { 'Content-type': 'application/json; charset=UTF-8' }
        })
        .then(r => r.json())
        .then(json => {
            if (json.status !== 400) {
                this.winningsTotalPulse = json;
            } else {
                this.winningsTotalPulse = null;
            }
            return;
        });
    },
    async GetTicketsBought(val) {
        let payload = { AccountNum: val };

        await fetch('api/NotALottery/GetTicketsBought', {
            method: 'POST',
            body: JSON.stringify(payload),
            headers: { 'Content-type': 'application/json; charset=UTF-8' }
        })
        .then(r => r.json())
        .then(json => {
            this.ticketsBought = json;
            return;
        });
    },
    async GetTotalTicketsBought() {
        await fetch('api/NotALottery/GetTotalTicketsBought', {
            method: 'Get',
            headers: { 'Content-type': 'application/json; charset=UTF-8' }
        })
        .then(r => r.json())
        .then(json => {
            this.totalTicketsBought = json;
            return;
        });
    },
    async GetStatistics() {
        await fetch('api/NotALottery/GetStatistics', {
            method: 'Get',
            headers: { 'Content-type': 'application/json; charset=UTF-8' }
        })
        .then(r => r.json())
        .then(json => {
            if (json.status !== 400) {
                this.statistics = json;
                this.statistics.totalPrizeMoneyDisplay = this.statistics.totalPrizeMoney.toLocaleString('en-US', { minimumFractionDigits: 0 });
                this.statistics.totalNumberPlayersDisplay = this.statistics.totalNumberPlayers.toLocaleString('en-US', { minimumFractionDigits: 0 });
            } else {
                this.statistics = null;
            }
            return;
        });
    },
    async GetEscrowAccountNum() {
        await fetch('api/NotALottery/GetEscrowAccountNum', {
            method: 'Get',
            headers: { 'Content-type': 'application/json; charset=UTF-8' }
        })
        .then(r => r.json())
        .then(json => {
            if (json.status !== 400) {
                this.escrowAccountNum = json.keyString;
            } else {
                this.escrowAccountNum = null;
            }
            return;
        });
    },
    async ClaimWinnings() {
        if (this.winningsTotalPulse === null || this.winningsTotalPulse === 0) {return;}

        this.winningsTotalPulse = 0;
        let payload = { AccountNum: this.currentAddress };

        this.doConfetti();

        await fetch('api/NotALottery/ClaimWinnings', {
            method: 'POST',
            body: JSON.stringify(payload),
            headers: { 'Content-type': 'application/json; charset=UTF-8' }
        })
        .then(r => r.json())
        .then(json => {
            if (json.status !== 400) {
                this.winningsTotalPulse = json;
            } else {
                this.winningsTotalPulse = null;
            }
            return;
        });
    },
    async BoughtLotto() {
        let vm = this;
        this.youBoughtLottoTicket = true;
        this.doConfetti();
        setTimeout(() => {
            vm.youBoughtLottoTicket = false;
        }, 10000)
    },
    async RefreshLotto() {
        setTimeout(() => {
            this.GetLottoWinnerLists();
            this.GetTotalTicketsBought();
            this.GetWinnings(this.currentAddress);
        }, 5000)
    },
    async BoughtPowerball() {
        let vm = this;
        this.youBoughtPowerballTicket = true;
        this.doConfetti();
        setTimeout(() => {
            vm.youBoughtPowerballTicket = false;
        }, 10000)
    },
    async RefreshPowerball() {
        setTimeout(() => {
            this.GetPowerballWinnerLists();
            this.GetTotalTicketsBought();
            this.GetTotalPlsList();
            this.GetWinnings(this.currentAddress);
        }, 5000)
    },
    async doConfetti() {
        // eslint-disable-next-line no-undef
        confetti({
            particleCount: 5000,
            spread: 1000,
            origin: { y: .6 }
        });
    },
    reload() {
        window.location.reload();
    },
  },
  computed: {
    metaMaskDownloaded() {
        if (window.ethereum !== null && window.ethereum !== undefined) {
            return true;
        } else {
            return false;
        }
    },
    showMyPrizeMoney() {
        return this.winningsTotalPulse !== null && this.winningsTotalPulse > 0;
    },
    canClaimPrize() {
        return this.chainId !== this.pulseChainId || this.winningsTotalPulse === null || this.winningsTotalPulse === 0;
    },
    showLottos() {
      return (
        this.lottoTypes !== null &&
        this.lottoTimes !== null &&
        this.ticketsBought !== null &&
        this.totalTicketsBought !== null &&
        this.totalPlsList !== null &&
        this.lottoWinnerLists !== undefined &&
        this.lottoWinnerLists !== null &&
        this.escrowAccountNum !== null
      );
    },
    showPowerball() {
      return (
        this.lottoTypes !== null &&
        this.lottoTimes !== null &&
        this.ticketsBought !== null &&
        this.totalTicketsBought !== null &&
        this.totalPlsList !== null &&
        this.powerballWinnerLists !== null &&
        this.escrowAccountNum !== null
      );
    },
    currentAddressDisplay() {
        if (this.currentAddress !== null) {
            return this.currentAddress.slice(0, 5) + '...' + this.currentAddress.slice(this.currentAddress.length - 5, this.currentAddress.length);
        } else {
            return null;
        }
    },
  },
  mounted() {
    setTimeout(() => {
        this.AccountFound();
    }, 1000)

    this.InitializePage();
  },
}
</script>

<style scoped>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  height: 100vh;
}
.center {
    text-align: center;
}
.white {
    color: white;
}
.highlight-btn {
    background-color: #6c757d;
    border: 1px solid white;
}
.pointer {
    cursor: pointer;
}
.inline {
    display: inline-block;
}
.page-header {
    margin-top: 30vh;
    padding: 0px 30px;
    color: white;
}
.my-navbar-btns {
    margin: 10px 0px;
}
@media (min-width: 768px) { 
    .page-header {
        margin-top: 10vh;
    }
    .my-navbar-btns {
        margin: 0px;
    }
 }
.connect-btn {
    border: 1px solid white !important;
    font-size: 12px;
}
.connected-btn {
    border: 1px solid white !important;
}
.download-mm-btn {
    cursor: pointer;
    color: #000;
    background-color: #ffc107;
    border-color: #ffc107;
    padding: 6px 12px 10px;
    border-radius: 5px;
    text-decoration: none;
}
.download-mm-btn:hover {
    color: #000;
    background-color: #ffca2c;
    border-color: #ffc720;
}
.download-mm-btn:focus {
    --bs-btn-focus-shadow-rgb: 217,164,6;
}
.download-mm-btn:active {
    color: #000;
    background-color: #ffcd39;
    border-color: #ffc720;
    --bs-btn-active-shadow: inset 0 3px 5px rgba(0, 0, 0, 0.125);
}
.download-mm-btn:disabled {
    color: #000;
    --bs-btn-disabled-bg: #ffc107;
    --bs-btn-disabled-border-color: #ffc107;
}
</style>
