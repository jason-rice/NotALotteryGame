<template>
  <div class="col">
    <div class="card">
      <div class="card-header cards-color-scheme border-white">
        <h5>{{ lottoType.type }}</h5>
      </div>
      <div class="card-body cards-color-scheme body-border">
        <div class="card-text prize-text">Prize: {{ totalPls }} PLS!!!</div>
        <div><p class="card-text" :style="timeLeft === 'Calculating winner!!' ? 'color:indianred' : 'color:white'">{{ timeLeft }}</p></div>
        <br />
        <div>
          <input type="number" class="buy-input" v-model="ticketNum" :min="1" :max="10000" step="1" :disabled="canBuyTickets" />
          <button type="button" class="btn btn-success buy-btn" @click="SendPulse()" :disabled="canBuyTickets">Buy Ticket(s)</button>
        </div>
        <br />
        <div><p class="card-text">Tickets bought: {{ ticketsBought }}</p></div>
        <div><p class="card-text">Total tickets sold: {{ totalTicketsBought }}</p></div>
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
      ticketNum: 1,
      ticketsBought: this.ticketsBoughtIncoming,
      totalTicketsBought: this.totalTicketsBoughtIncoming,
      totalPls: this.totalPlsIncoming.toLocaleString('en-US', { minimumFractionDigits: 0 }),
      escrowAccountNum: this.escrowAccountNumIncoming,
      winnersList: this.winners,
    };
  },
  methods: {
    InitializePage() {
      this.BeginCountdown(this.timeEnd);
      this.ShowWinnersListStrings();
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
            clearInterval(x);
            vm.timeLeft = "Calculating winner!!";
            vm.ticketsBought = 0;
            vm.totalPls = 0;
            vm.$emit("refreshLotto");
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
              value: Number(20000000000000000000000 * this.ticketNum).toString(16),
            },
          ],
        })
        .then((txHash) => vm.BuyTickets(txHash))
        .catch((error) => console.error(error));
    },
    async BuyTickets(txHash) {
      if (this.ticketNum > 0 && this.ticketNum <= 10000) {
        let payload = {
          AccountNum: this.currentAddress,
          TicketNum: this.ticketNum,
          Type: this.lottoType.id,
          TxHash: txHash,
        };

        await fetch("api/NotALottery/BuyLottoTickets", {
          method: "POST",
          body: JSON.stringify(payload),
          headers: { "Content-type": "application/json; charset=UTF-8" },
        })
        .then((r) => r.json())
        .then((json) => {
          if (json.status !== 400) {
            if (json > 0) {
              this.ticketsBought = json;
                this.$emit("youBoughtLotto");
            } else {
              console.log("error during buy");
            }
          } else {
              this.ticketsBought = 0;
          }
          return;
        });
      }
    },
    ShowWinnersListStrings() {
      if (this.winnersList !== undefined && this.winnersList.length > 0) {
        this.winnersList.forEach(winner => {
          winner.displayString = winner.addressId.slice(0, 5) + '...' + winner.addressId.slice(winner.addressId.length - 5, winner.addressId.length);
        });
      }
    }
  },
  watch: {
    ticketNum() {
      if (this.ticketNum === "" || this.ticketNum < 1) {
        this.ticketNum = 1;
      } else if ( this.ticketNum > 10000) {
        this.ticketNum = 10000;
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
.buy-input {
  display: inline-block;
  width: 25%;
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
