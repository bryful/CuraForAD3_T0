# CuraForAD3_T0

3Dプリンタでの必需品であるスライサーのUltimaker Curaを使ってみたら非常にいい感じで、FlashForgeのAdventurer3でも使えないかなと作ったものです。<br>
[SatoMouse's blogのAdventurer3でCuraを使う](https://satomouse.com/adventurer3-cura/)にあったものをそのままC#で作ったものです。<br>
<br>
本当はCuraのプラグインで対処したかったのですが、資料が見つからなかったのでこれを作りました。<br>
<br>
Curaの書き出すgcodeファイルはヒートベッドの温度設定のM140/M104行の末尾にT0が付かないので付けるアプリです。<br>

<hr>

## Dependency
Visual studio 2019 C#



## License
This software is released under the MIT License, see LICENSE

## Authors

bry-ful(Hiroshi Furuhashi)<br>
twitter:[bryful](https://twitter.com/bryful)<br>
bryful@gmail.com<br>

## References
[SatoMouse's blogのAdventurer3でCuraを使う](https://satomouse.com/adventurer3-cura/)