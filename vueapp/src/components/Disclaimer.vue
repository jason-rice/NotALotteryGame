<!-- eslint-disable vue/multi-word-component-names -->
<template>
    <div>
        <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content my-card-color-scheme">
                    <div class="modal-header">
                        <div class="row my-font-size-large">Terms of service</div>
                    </div>
                    <div class="modal-body my-card-body" style="text-align: start;">
                        <div class="row my-font-size-medium">Before utilizing this website, we strongly advise you to carefully review our Terms of Service.</div>
                        <br>
                        <div class="row my-font-size-small">
                            <div class="row">1. Responsible Gaming</div>
                            <div class="row">2. Limitation of Liability and Complaints</div>
                            <div class="row">3. Eligibility for Usage</div>
                            <div class="row">4. Termination</div>
                            <div class="row">5. No Guarantees</div>
                        </div>
                        <br>
                        <div class="row my-font-size-medium">Responsible Gaming</div>
                        <br>
                        <div class="row my-font-size-small">
                            Responsible gaming encompasses a set of socially responsible initiatives within the industry to ensure the fairness and integrity of its operations. It also strives to raise awareness about potential harms associated with gaming. Here are some guidelines on how to play responsibly:
                        </div>
                        <br>
                        <div class="row my-font-size-small">
                            <div class="row">- Do not view using Not A Lottery as a means to make money; consider it an entertainment expense.</div>
                            <div class="row">- Only play with disposable income that you can afford to lose.</div>
                            <div class="row">- Avoid chasing losses; there is no guarantee of coming out ahead.</div>
                            <div class="row">- Refrain from playing under the influence of alcohol, drugs, or other substances.</div>
                            <div class="row">- Do not play when your emotions are unmanageable, such as when you're feeling depressed or upset.</div>
                            <div class="row">- Remember to take regular breaks or set a predetermined playing time limit.</div>
                        </div>
                        <br>
                        <div class="row my-font-size-medium">Limitation of Liability</div>
                        <div class="row my-font-size-small">Not A Lottery is not responsible for any financial losses resulting from website usage due to circumstances beyond our control. We bear no responsibility for unauthorized access to a user's account or losses resulting from illicit use of a user's password or transactions where a user's login credentials were accurately provided.</div>
                        <br>
                        <div class="row my-font-size-small">We are not liable for technical issues, including hardware or software defects, unstable internet connections, or other technical errors affecting user access to the website. Additionally, we cannot guarantee that the software or website will always function flawlessly, be free from errors, or accessible without interruptions.</div>
                        <br>
                        <div class="row my-font-size-small">You acknowledge that Not A Lottery has the final say in determining whether you have violated our rules, terms, or conditions, leading to suspension or permanent exclusion from our website. Our website is intended for personal use and should not be used for commercial purposes.</div>
                        <br>
                        <div class="row my-font-size-small">Except where legally required, Not A Lottery, its directors, employees, partners, and service providers are not liable for special, indirect, or consequential damages or any other damages arising from the use or inability to use our services. This includes but is not limited to loss of use, profits, or data, whether due to contract, tort, negligence, or other causes. Some jurisdictions may not permit the exclusion of certain warranties or limitations of liability for incidental or consequential damages, so some of these limits may not apply to certain users.</div>
                        <br>
                        <div class="row my-font-size-small">Not A Lottery is not liable for damages resulting from delayed or failed performance of obligations due to events such as fires, strikes, floods, power outages, acts of God or the state's enemies, lawful acts of public authorities, or force majeure events.</div>
                        <br>
                        <div class="row my-font-size-medium">Eligibility to Use Not A Lottery's Services</div>
                        <br>
                        <div class="row my-font-size-small">1. The website and its services are available only to individuals who can enter legally binding contracts under applicable law and reside in regions where online activities are legally allowed. It is your responsibility to understand the local laws and regulations before making purchases on the website.</div>
                        <br>
                        <div class="row my-font-size-small">2. You guarantee that you are at least 18 years old or the legal age of majority in your jurisdiction. We may request proof of age. You must be a natural person (not a legal entity or corporation) who can enter into legally binding contracts. You must reside in areas where the services are legally permitted.</div>
                        <br>
                        <div class="row my-font-size-small">3. You also guarantee that you are not a resident or citizen of countries where our services are prohibited by local legislation. Players from such countries are not eligible to use the website and its services. We will make reasonable efforts to prevent players from these countries from accessing our services.</div>
                        <br>
                        <div class="row my-font-size-medium">Termination</div>
                        <br>
                        <div class="row my-font-size-small">We reserve the right to terminate or suspend access to our service at our discretion and without prior notice or liability, should you violate these terms. Provisions in these terms that, by their nature, should survive termination, will remain in effect.</div>
                        <br>
                        <div class="row my-font-size-medium">No Guarantees</div>
                        <br>
                        <div class="row my-font-size-small">This website is provided 'as is' without any explicit or implied representations or warranties. Not A Lottery makes no representations or warranties concerning the availability of the website or the accuracy and completeness of the information provided. The information on this website is not guaranteed to be constantly available or accurate.</div>
                        <br>
                        <div class="row my-font-size-small">Nothing on this website should be considered as advice of any kind. For legal, financial, or medical matters, it is recommended to consult with the appropriate professionals.</div>
                        <br>
                        <div class="row my-font-size-small">You acknowledge that your use of the website is solely at your own discretion, volition, and risk.</div>
                        <br>
                        <div><input type="checkbox" v-model="iAgree"> I am 18+ years old, and I accept the terms of service provided</div>
                    </div>
                    <div class="modal-footer">
                        <div><button type="button" class="btn btn-primary" data-bs-dismiss="modal" :disabled="!iAgree" @click="ShowHideIntructions(false)">Continue</button></div>
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
                iAgree: false,
            };
        },
        methods: {
            ShowHideIntructions(val) {
                localStorage.setItem('disclaimer', val);
            },
        },
        mounted() {
            // eslint-disable-next-line no-undef
            let myModal = new bootstrap.Modal($("#exampleModal").modal(), {
                backdrop: 'false',
                keyboard: false,
                focus: true,
            });

            if (localStorage.getItem('disclaimer') === 'true') {
                
                myModal.show();
            } else if (localStorage.getItem('disclaimer') === 'false') {
                myModal.hide();
            }
            
        },
        created() {
            if (localStorage.getItem('disclaimer') === null) {
                localStorage.setItem('disclaimer', 'true');
            }
        }
    };
</script>
  
<style scoped>
.my-card-color-scheme {
  background-color: #202022;
  color: grey;
}
.my-card-body {
    overflow-y: scroll;
    height: 200px;
}
.my-font-size-small {
    font-size: xx-small;
}
.my-font-size-medium {
    font-size: small;
}
.my-font-size-large {
    font-size: larger;
}
</style>
  