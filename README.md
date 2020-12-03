__---This project is no longer in development and is provided for archive purposes only---__

# T.I.M - Thrust Indication Module: A mod for Kerbal Space Program

T.I.M allows you to easily visualize your current thrust output and efficiency using the stock GUI. T.I.M. displays a second needle in the throttle gauge next to the NavBall and shows you the current thrust output as a percentage of the engine(s) total capable thrust.  Very handy for Planes or anywhere you find an atmosphere.

### Features in v1.1

* Shows current, combined Thrust Output of all active engines on vessel:
  * The gray needle in the throttle gauge shows the current percent thrust, just like the throttle percentage.
  * Works with all Engines
* Thrust Calculations have been updated to the following model:
  * For Jet Engines, VelCurve and AtmCurve are evaluated and depending on the resulting Thrust Multiplier, the following calculations are used:
    * If the resulting Thrust Multiplier is greater than 1.0, Maximum Thrust Multiplier is multiplied by maximum engine thrust in part cfg.
    * If the resulting Thrust Multiplier is less than 1.0, only the maximum engine thrust from the part cfg is used.
  * All other engines use the maximum engine thrust from the part cfg.
* Two different indicator styles are now available:
  * Stock mode with two needles (as shown in the above screenshots).
    * Throttle Needle is white, Thrust Needle is grey.
    * The opacity of the Throttle Needle will now fade when near the Thrust Needle so you can see it better.  This fade amount can be configured in a config file.
  * Split Needle mode (as shown below).
    * This shows one needle similar to the stock needle when Thrust = Throttle. 
    * Shows a split needle when values are different, Throttle (top half) and Thrust (bottom half).

To Change Needle Types Open: GameData\ThrustIndicationModule\Config\ThrustIndicationModule_PluginSettings.cfg

There are comments inside this file, but...

To use the split needle instead of two separate needles change 'UseSplitNeedle' to 'true'.  To use the dual Needles, change 'UseSplitNeedle' to 'false'.

To change the opacity of the Throttle Needle when in stock needle mode change 'StockGaugeColorAlpha' between '0.00' and '1.00'. 0 is completely clear, 1 is completely opaque. 
