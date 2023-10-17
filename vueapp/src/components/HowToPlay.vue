<template>
    <div>
        <div>
            <button type="button" class="btn btn-primary" v-show="displayHowTo === 'true'" @click="ShowHideIntructions('false')">
                Hide instructions?
            </button>
        </div>
        <div>
            <button type="button" class="btn btn-primary" v-show="displayHowTo === 'false'" @click="ShowHideIntructions('true')">
                How to play?
            </button>
        </div>
        <br>
        <div v-show="displayHowTo === 'true'">
            <div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-lg-3 row-cols-xl-3 row-cols-xxl-6 g-5" style="padding: 1% 10% 5%">
                <div class="col">
                    <div class="card">
                        <div class="card-header cards-color-scheme border-white"><h1>Step 1</h1></div>
                        <div class="card-body cards-color-scheme body-border">
                            <img decoding="async" width="138" height="138" src="https://myteddybear.io/wp-content/uploads/2023/09/Untitled-design-2023-03-30T212906_738.webp">
                            <div class="title-font">Create a Wallet</div>
                            <div class="body-font">
                                Download metamask or your wallet of choice from the app store or google play store for free. Desktop users, download the google chrome extension by going to metamask.io.
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col">
                    <div class="card">
                        <div class="card-header cards-color-scheme border-white"><h2>Step 2</h2></div>
                        <div class="card-body cards-color-scheme body-border">
                            <img decoding="async" width="138" height="138" src="https://myteddybear.io/wp-content/uploads/2023/09/Untitled-design-2023-03-30T213620_680.webp">
                            <div class="title-font">Get Some ETH</div>
                            <div class="body-font">
                                Have ETH in your wallet to swap. If you don’t have any ETH, you can buy directly on metamask, transfer from another wallet, or buy on another exchange and send it to your wallet.
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col">
                    <div class="card">
                        <div class="card-header cards-color-scheme border-white"><h3>Step 3</h3></div>
                        <div class="card-body cards-color-scheme body-border">
                            <img decoding="async" width="138" height="138" src="https://myteddybear.io/wp-content/uploads/2023/09/uniswap-2-1.webp">
                            <div class="title-font">Go to Uniswap</div>
                            <div class="body-font">
                                Connect to Uniswap. Go to app.uniswap.org in google chrome or on the browser inside your Metamask app. Connect your wallet. Paste the WPLS token address into Uniswap, and confirm. When Metamask prompts you for a wallet signature, sign.
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col">
                    <div class="card">
                        <div class="card-header cards-color-scheme border-white"><h4>Step 4</h4></div>
                        <div class="card-body cards-color-scheme body-border">
                            <img decoding="async" width="138" height="138" src="https://myteddybear.io/wp-content/uploads/2023/09/Untitled-design-2023-03-30T213620_680.webp">
                            <div class="title-font">Buy some WPLS on Ethereum</div>
                            <div class="body-font">
                                Use The contract code to buy some Pulsechain on Ethereum

                                0xd6c31bA0754C4383A41c0e9DF042C62b5e918f6d
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col">
                    <div class="card">
                        <div class="card-header cards-color-scheme border-white"><h5>Step 5</h5></div>
                        <div class="card-body cards-color-scheme body-border">
                            <img decoding="async" width="138" height="138" src="https://myteddybear.io/wp-content/uploads/2023/09/pulsechain.png">
                            <div class="title-font">Bridge Your WPLS to Pulsechain</div>
                            <div class="body-font">
                                Go to https://bridge.pulsechain.com Take your WPLS to turn it into PLS This may take 5-15 minutes depending on traffic of incoming funds.
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col">
                    <div class="card">
                        <div class="card-header cards-color-scheme border-white"><h6>Step 6</h6></div>
                        <div class="card-body cards-color-scheme body-border">
                            <p class="body-font">
                                Congrats! You're now ready to play the game! Just click the buy button on the lotto of your choice! Good luck, have FUN!!!
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
  
  
<script>
    export default {
        data() {
            return {
                displayHowTo: null,
            };
        },
        methods: {
            ShowHideIntructions(val) {
                localStorage.setItem('displayHowTo', val);
                this.displayHowTo = localStorage.getItem('displayHowTo');
            },
        },
        mounted() {
            localStorage.removeItem('displayHowTo');
            if (localStorage.getItem('displayHowTo') === null) {
                localStorage.setItem('displayHowTo', 'false');
            }
            this.displayHowTo = localStorage.getItem('displayHowTo');
        }
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
.title-font {
    font-size: larger;
    font-weight: bold;
}
.body-font {
    font-size: medium;
    font-weight: normal;
}
</style>
  