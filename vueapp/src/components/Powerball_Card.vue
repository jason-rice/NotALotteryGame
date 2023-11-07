<template>
    <div class="col" style="text-align: -webkit-center;">
      <div class="card" style="max-width: 800px;">
        <div class="card-header cards-color-scheme border-white">
          <h5>{{ lottoType.type }}</h5>
        </div>
        <div class="card-body cards-color-scheme body-border">
          <div class="card-text prize-text">Prize: {{ totalPls }} PLS!!!</div>
          <div><p class="card-text" :style="timeLeft === 'Calculating winner!!' ? 'color:indianred' : 'color:white'">{{ timeLeft }}</p></div>
          <br />
          <div>
            <button type="button" class="btn btn-success buy-btn" @click="choosingNums = true" :disabled="canBuyTickets">
                Choose Numbers
            </button>
            <div v-if="choosingNums === true">
                <br>
                <div class="container" style="padding: 0px 15%;">
                    <div class="row row-cols-1 row-cols-sm-1 row-cols-md-3 row-cols-lg-3 row-cols-xl-3 g-4">
                        <div class="col">
                            <input type="number" class="buy-input" v-model="num1" :min="1" :max="99" step="1" :disabled="canBuyTickets" />
                        </div>
                        <div class="col">
                            <input type="number" class="buy-input" v-model="num2" :min="1" :max="99" step="1" :disabled="canBuyTickets" />
                        </div>
                        <div class="col">
                            <input type="number" class="buy-input" v-model="num3" :min="1" :max="99" step="1" :disabled="canBuyTickets" />
                        </div>
                    </div>
                </div>
                <br>
                <button type="button" class="btn btn-success buy-btn" @click="SendPulse()" :disabled="canBuyTickets || cannotSubmit">
                    Buy Ticket
                </button>
            </div>
          </div>
          <br />
          <div><p class="card-text">Tickets bought: {{ ticketsBought }}</p></div>
          <div>
            <p class="card-text">Total tickets sold: {{ totalTicketsBought }}</p>
        </div>
        <div>
            <br>
            <p>Your numbers:</p>
            <div class="container" style="padding: 0px 15%;">
                <div class="row row-cols-1 row-cols-sm-1 row-cols-md-3 row-cols-lg-3 row-cols-xl-3 g-4">
                    <div v-for="ticket in tickets" :key="ticket.id" class="col">
                        <div class="card">
                            <div class="card-body cards-color-scheme body-border">
                                <div class="inline">{{ ticket.numOne }}</div>
                                <div class="inline" style="margin: 0px 10px;">{{ ticket.numTwo }}</div>
                                <div class="inline">{{ ticket.numThree }}</div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        </div>
        <div class="card-footer cards-color-scheme border-white">
          Last five winners:
          <div v-for="win in winnersList" :key="win.id" class="winners-list row">
              <div class="left address-str">{{ win.displayString }}</div>
              <div class="right pls-str">{{ win.amountPulse }} PLS!!!</div>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  
  export default {
    props: {
      lottoType: null,
      timeEnd: null,
      ticketsBoughtIncoming: null,
      totalTicketsBoughtIncoming: null,
      totalPlsIncoming: null,
      escrowAccountNumIncoming: null,
      winners: null,
    },
    data() {
      return {
        nextLotteryTime: null,
        timeLeft: null,
        tickets: null,
        ticketsBought: this.ticketsBoughtIncoming,
        totalTicketsBought: this.totalTicketsBoughtIncoming,
        totalPls: this.totalPlsIncoming.toLocaleString('en-US', { minimumFractionDigits: 0 }),
        escrowAccountNum: this.escrowAccountNumIncoming,
        winnersList: this.winners,
        choosingNums: false,
        num1: null,
        num2: null,
        num3: null,
      };
    },
    methods: {
      InitializePage() {
        this.BeginCountdown(this.timeEnd);
        this.ShowWinnersListStrings();
        this.GetMyPowerballTicketsBought(this.currentAddress);
      },
      async AccountFound() {
          if (window.ethereum !== null && window.ethereum !== undefined) {
            this.accountNum = window.ethereum.selectedAddress;
            this.chainId = window.ethereum.chainId;
          }
      },
      async BeginCountdown(val) {
        let vm = this;
        vm.nextLotteryTime = new Date(val).getTime();
  
        var x = setInterval(function () {
          if (val !== null && val !== undefined) {
            let a = new Date(new Date().toLocaleString("en-US", { timeZone: "America/Chicago" })).getTime();
  
            var distance = vm.nextLotteryTime - a;
  
            var days = Math.floor(distance / (1000 * 60 * 60 * 24));
            var hours = Math.floor(
              (distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60)
            );
            var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
            var seconds = Math.floor((distance % (1000 * 60)) / 1000);
  
            vm.timeLeft = days + "d " + hours + "h " + minutes + "m " + seconds + "s ";
  
            // If the count down is over, write some text
            if (distance < 5000) {
            //   clearInterval(x);
              vm.timeLeft = "Calculating winner!!";
            //   vm.ticketsBought = 0;
            //   vm.totalPls = 0;
            //   vm.$emit("refreshPowerball");
            }
            if (distance < 1000) {
              clearInterval(x);
              vm.timeLeft = "Calculating winner!!";
              vm.ticketsBought = 0;
            //   vm.totalPls = 0;
              vm.$emit("refreshPowerball");
            }
          }
        }, 1000);
      },
      async SendPulse() {
        let vm = this;
        window.ethereum
          .request({
            method: "eth_sendTransaction",
            params: [
              {
                from: this.currentAddress,
                to: this.escrowAccountNum, // wallet 3.2
                value: Number(20000000000000000000000).toString(16),
              },
            ],
          })
          .then((txHash) => vm.BuyTickets(txHash))
          .catch((error) => console.error(error));
      },
      async BuyTickets(txHash) {
        if (this.num1 > 0 && this.num1 <= 99 && this.num2 > 0 && this.num2 <= 99 && this.num3 > 0 && this.num3 <= 99) {
          let payload = {
            AccountNum: this.currentAddress,
            NumOne: this.num1,
            NumTwo: this.num2,
            NumThree: this.num3,
            TxHash: txHash,
          };
  
          await fetch("api/NotALottery/BuyPowerballTicket", {
            method: "POST",
            body: JSON.stringify(payload),
            headers: { "Content-type": "application/json; charset=UTF-8" },
          })
          .then((r) => r.json())
          .then((json) => {
            if (json.status === 200) {
                this.tickets = json;
                this.ticketsBought = json.length;
                this.$emit("youBoughtPowerball");
            } else {
                this.tickets = json;
                this.ticketsBought = 0;
            }
            return;
          });
        }
      },
    async GetMyPowerballTicketsBought(val) {
        let payload = { AccountNum: val };

        await fetch('api/NotALottery/GetMyPowerballTicketsBought', {
            method: 'POST',
            body: JSON.stringify(payload),
            headers: { 'Content-type': 'application/json; charset=UTF-8' }
        })
        .then(r => r.json())
        .then(json => {
            this.tickets = json;
            return;
        });
    },
      ShowWinnersListStrings() {
        this.winnersList.forEach(winner => {
          winner.displayString = winner.addressId.slice(0, 5) + '...' + winner.addressId.slice(winner.addressId.length - 5, winner.addressId.length);
        });
      }
    },
    watch: {
        num1() {
            if (this.num1 === "" || this.num1 < 1) {
                this.num1 = 1;
            } else if ( this.num1 > 99) {
                this.num1 = 99;
            }
        },
        num2() {
            if (this.num2 === "" || this.num2 < 1) {
                this.num2 = 1;
            } else if ( this.num2 > 99) {
                this.num2 = 99;
            }
        },
        num3() {
            if (this.num3 === "" || this.num3 < 1) {
                this.num3 = 1;
            } else if ( this.num3 > 99) {
                this.num3 = 99;
            }
        },
        timeEnd() {
        this.BeginCountdown(this.timeEnd);
        },
        ticketsBoughtIncoming() {
        this.ticketsBought = this.ticketsBoughtIncoming;
        },
        totalTicketsBoughtIncoming() {
        this.totalTicketsBought = this.totalTicketsBoughtIncoming;
        },
        totalPlsIncoming() {
        this.totalPls = this.totalPlsIncoming.toLocaleString('en-US', { minimumFractionDigits: 0 });
        },
        escrowAccountNumIncoming() {
        this.escrowAccountNum = this.escrowAccountNumIncoming;
        },
        winners() {
            this.winnersList = this.winners;
            this.ShowWinnersListStrings();
        },
    },
    computed: {
      canBuyTickets() {
        return this.timeLeft === 'Calculating winner!!' || this.currentAddress === null || this.chainId === null || this.chainId !== '0x171';
      },
      cannotSubmit() {
        return this.num1 === null || this.num2 === null || this.num3 === null;
      },
      currentAddress() {
        if (window.ethereum !== null && window.ethereum !== undefined) {
          return window.ethereum.selectedAddress;
        } else {
          return null;
        }
      },
      chainId() {
        if (window.ethereum !== null && window.ethereum !== undefined) {
          return window.ethereum.chainId;
        } else {
          return null;
        }
      },
    },
    mounted() {
      this.InitializePage();
    },
  };
  </script>
  
  <style scoped>
  .cards-color-scheme {
    background-color: #202022;
    color: white;
  }
  .border-white {
    border: 1px solid white;
  }
  .body-border {
    border-left: 1px solid white;
    border-right: 1px solid white;
  }
  .winners-list {
    font-size: 15px;
    margin: 0px;
  }
  .left {
    text-align: left;
  }
  .right {
    text-align: right;
  }
  .inline {
    display: inline-block;
  }
  .buy-input {
    display: inline-block;
    width: 100%;
  }
  .buy-btn {
    display: inline-block;
    margin-left: 15px;
  }
  .prize-text {
    color:#ffc107;
    font-size: 20px;
  }
  .address-str {
    width: 45%;
    font-size: 12px;
    padding: 0px;
  }
  .pls-str {
    width: 45%;
    font-size: 12px;
    padding: 0px;
  }
  </style>
  